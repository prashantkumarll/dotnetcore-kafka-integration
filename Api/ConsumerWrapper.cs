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

            // Create the ServiceBusProcessor instance from the client
            this._processor = client.CreateProcessor(this._topicName, options ?? new ServiceBusProcessorOptions());

            // Register handlers
            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ProcessErrorHandler;

            // Start processing in the background
            _processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            // Use a short timeout so this method doesn't block indefinitely.
            // You can adjust the timeout or add an overload that accepts CancellationToken.
            _tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            using (cts.Token.Register(() => _tcs.TrySetResult(null)))
            {
                try
                {
                    return await _tcs.Task.ConfigureAwait(false);
                }
                finally
                {
                    // ensure tcs cleared for next call
                    Interlocked.Exchange(ref _tcs, null);
                }
            }
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();
                var tcs = Interlocked.Exchange(ref _tcs, null);
                if (tcs != null)
                {
                    tcs.TrySetResult(body);
                }

                // Complete the message so it's removed from the queue
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            }
            catch
            {
                // swallow to prevent processor crash; error handler will be invoked
            }
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            // Log or handle the error as required
            return Task.CompletedTask;
        }

        /// <summary>
        /// Properly close and dispose the consumer.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                // Attempt to stop processing cleanly
                _processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on close
            }
            _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
