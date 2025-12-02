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

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Create the ServiceBusProcessor for the given topic/queue name
            this._processor = client.CreateProcessor(this._topicName, options ?? new ServiceBusProcessorOptions());

            // Do not start processing here; readMessage will start/stop around a single read.
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            return readMessageAsync().GetAwaiter().GetResult();
        }

        private async Task<string> readMessageAsync()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    // Complete the message and set the result for the waiting task
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                    tcs.TrySetResult(body);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                // Optionally log args.Exception
                return Task.CompletedTask;
            }

            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            try
            {
                await _processor.StartProcessingAsync().ConfigureAwait(false);

                var delay = Task.Delay(TimeSpan.FromSeconds(1));
                var completed = await Task.WhenAny(tcs.Task, delay).ConfigureAwait(false);

                if (completed == tcs.Task)
                {
                    return await tcs.Task.ConfigureAwait(false);
                }
                else
                {
                    return null;
                }
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception)
            {
                // log depending on strategy
                return null;
            }
            finally
            {
                try
                {
                    await _processor.StopProcessingAsync().ConfigureAwait(false);
                }
                catch
                {
                    // ignore stop errors
                }
                _processor.ProcessMessageAsync -= MessageHandler;
                _processor.ProcessErrorAsync -= ErrorHandler;
            }
        }

        /// <summary>
        /// Properly close and dispose the processor.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                _processor?.CloseAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on close
            }

            try
            {
                _processor?.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore dispose errors
            }

            _disposed = true;
        }
    }
}
