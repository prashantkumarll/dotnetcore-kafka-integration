namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusProcessor _processor;
        private TaskCompletionSource<string> _tcs;
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            var svcClient = client ?? throw new ArgumentNullException(nameof(client));

            // Create the processor for the provided topic
            this._processor = svcClient.CreateProcessor(this._topicName, options ?? new ServiceBusProcessorOptions());

            // Register handlers
            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;

            // Start processing (synchronously block until started)
            this._processor.StartProcessingAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            try
            {
                // Prepare a TaskCompletionSource that will be completed by the message handler
                var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
                Interlocked.Exchange(ref _tcs, tcs);

                var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(1)));
                if (completed != tcs.Task)
                {
                    // timeout - clear tcs and return null
                    Interlocked.Exchange(ref _tcs, null);
                    return null;
                }

                return await tcs.Task;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception)
            {
                // log or rethrow depending on your logging strategy
                return null;
            }
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();

                var tcs = Interlocked.Exchange(ref _tcs, null);
                if (tcs != null && !tcs.Task.IsCompleted)
                {
                    tcs.TrySetResult(body);
                }

                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            }
            catch
            {
                // ignore handler exceptions, ProcessErrorAsync will be invoked for errors
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            try
            {
                var tcs = Interlocked.Exchange(ref _tcs, null);
                if (tcs != null && !tcs.Task.IsCompleted)
                {
                    tcs.TrySetResult(null);
                }
            }
            catch
            {
                // ignore
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Properly close and dispose the processor.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                // Stop processing and dispose
                _processor.StopProcessingAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }

            try
            {
                _processor.ProcessMessageAsync -= MessageHandler;
                _processor.ProcessErrorAsync -= ErrorHandler;
            }
            catch
            {
                // ignore
            }

            _processor.DisposeAsync().AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
