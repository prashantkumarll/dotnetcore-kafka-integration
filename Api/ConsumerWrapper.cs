namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        private TaskCompletionSource<string> _tcs;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            var opts = options ?? new ServiceBusProcessorOptions();

            // Create the processor from the ServiceBusClient
            this._processor = this._client.CreateProcessor(this._topicName, opts);

            // Register handlers
            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;

            // Start processing messages
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();

                // Complete the message
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);

                // Signal any waiting reader
                var tcs = _tcs;
                if (tcs != null)
                {
                    tcs.TrySetResult(body);
                }
            }
            catch
            {
                // swallow to allow processor to continue
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // You can log the error here. If a reader is waiting, signal null to indicate failure/no message.
            var tcs = _tcs;
            if (tcs != null)
            {
                tcs.TrySetResult(null);
            }
            return Task.CompletedTask;
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
                _tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
                var completedTask = Task.WhenAny(_tcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).GetAwaiter().GetResult();
                if (completedTask != _tcs.Task) return null;
                return _tcs.Task.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                // consumer was cancelled/closed - treat as no message
                return null;
            }
            catch (Exception)
            {
                // log or rethrow depending on your logging strategy
                return null;
            }
        }

        /// <summary>
        /// Properly close and dispose the consumer.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                // Stop processing
                this._processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }

            this._processor.ProcessMessageAsync -= MessageHandler;
            this._processor.ProcessErrorAsync -= ErrorHandler;

            this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();

            _disposed = true;
        }
    }
}
