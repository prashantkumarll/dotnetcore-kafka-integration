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
        private static readonly Random rand = new Random();
        private bool _disposed = false;
        private TaskCompletionSource<string> _tcs;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));

            var opts = options ?? new ServiceBusProcessorOptions();

            // Build the ServiceBusProcessor instance from the client
            this._processor = client.CreateProcessor(this._topicName, opts);

            // Register handlers
            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            // Use a short timeout so this method doesn't block indefinitely.
            // You can adjust the timeout or add an overload that accepts CancellationToken.
            try
            {
                // Prepare a task completion source to receive the next message
                _tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

                // Start processing (event-driven)
                _processor.StartProcessingAsync().GetAwaiter().GetResult();

                var completed = Task.WhenAny(_tcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).GetAwaiter().GetResult();
                if (completed == _tcs.Task)
                {
                    return _tcs.Task.GetAwaiter().GetResult();
                }

                return null;
            }
            catch (OperationCanceledException)
            {
                // processor was cancelled/closed - treat as no message
                return null;
            }
            catch (Exception)
            {
                // log or rethrow depending on your logging strategy
                return null;
            }
            finally
            {
                try
                {
                    _processor.StopProcessingAsync().GetAwaiter().GetResult();
                }
                catch
                {
                    // ignore errors on stop
                }
            }
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();

                // Complete the message so it's not received again
                await args.CompleteMessageAsync(args.Message);

                // Atomically grab and clear the TaskCompletionSource so only one receiver completes it
                var tcs = Interlocked.Exchange(ref _tcs, null);
                if (tcs != null)
                {
                    tcs.TrySetResult(body);
                }
            }
            catch
            {
                // ignore handler exceptions
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // You may log the error here
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
                // Attempt to stop processing
                _processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }
            try
            {
                _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on dispose
            }

            _disposed = true;
        }
    }
}
