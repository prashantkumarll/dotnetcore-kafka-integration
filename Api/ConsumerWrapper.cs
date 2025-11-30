namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly BlockingCollection<string> _messageQueue = new BlockingCollection<string>();
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            var processorOptions = options ?? new ServiceBusProcessorOptions();

            // Build the ServiceBusProcessor instance from the client
            this._processor = this._client.CreateProcessor(this._topicName, processorOptions);

            // Register handlers to enqueue messages and handle errors
            this._processor.ProcessMessageAsync += async args =>
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    _messageQueue.Add(body);
                }
                catch
                {
                    // ignore individual message handling errors
                }
                await Task.CompletedTask;
            };

            this._processor.ProcessErrorAsync += args =>
            {
                // You can log args.Exception here if desired
                return Task.CompletedTask;
            };

            // Start the processor so messages will be received
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            try
            {
                // Attempt to take a message from the internal queue with a 1 second timeout
                var takeTask = Task.Run(() =>
                {
                    if (_messageQueue.TryTake(out var msg, 1000))
                    {
                        return msg;
                    }
                    return null;
                });

                return await takeTask.ConfigureAwait(false);
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
                this._processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }

            try
            {
                this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore dispose errors
            }

            _disposed = true;
        }
    }
}
