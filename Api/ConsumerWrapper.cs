namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusProcessor _processor;
        private volatile TaskCompletionSource<string> _currentTcs;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));

            var opts = options ?? new ServiceBusProcessorOptions();

            // Build the ServiceBusProcessor instance from the client
            this._processor = client.CreateProcessor(this._topicName, opts);

            // Register event handlers for message processing and errors
            this._processor.ProcessMessageAsync += this.ProcessMessageHandler;
            this._processor.ProcessErrorAsync += this.ProcessErrorHandler;

            // Start processing messages
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            // Use a short timeout so this method doesn't block indefinitely.
            // You can adjust the timeout or add an overload that accepts CancellationToken.
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            this._currentTcs = tcs;

            var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).ConfigureAwait(false);
            if (completed == tcs.Task)
            {
                return await tcs.Task.ConfigureAwait(false);
            }

            // Timeout - ensure we don't hold onto the tcs
            if (ReferenceEquals(this._currentTcs, tcs))
            {
                this._currentTcs = null;
            }

            return null;
        }

        private Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();

                var tcs = this._currentTcs;
                if (tcs != null)
                {
                    // Try to set the result without throwing if it's already completed
                    tcs.TrySetResult(body);
                }
            }
            catch
            {
                // ignore handler exceptions to prevent crashing the processor
            }

            return Task.CompletedTask;
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            // Log or handle the error as appropriate.
            // Swallowing the exception to mirror previous behavior where errors were ignored.
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
                // Attempt to stop processing messages cleanly
                _processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }
            _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
