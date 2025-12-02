namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();
        private readonly SemaphoreSlim _messageAvailable = new SemaphoreSlim(0);
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            options = options ?? new ServiceBusProcessorOptions();

            // Create the ServiceBusProcessor for the specified topic/queue name
            this._processor = this._client.CreateProcessor(this._topicName, options);

            // Register handlers for message processing and errors
            this._processor.ProcessMessageAsync += this.ProcessMessageHandler;
            this._processor.ProcessErrorAsync += this.ProcessErrorHandler;

            // Start processing messages (synchronously wait in ctor)
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message?.Body.ToString();
                if (body != null)
                {
                    _messageQueue.Enqueue(body);
                    _messageAvailable.Release();
                }

                // Rely on processor options (AutoCompleteMessages) for completion behavior.
                await Task.CompletedTask;
            }
            catch
            {
                // swallow here; errors are surfaced via ProcessErrorAsync
            }
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            // Log or handle errors appropriately. For now, swallow and continue.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            try
            {
                // Wait up to 1 second for a message to arrive
                if (!_messageAvailable.Wait(TimeSpan.FromSeconds(1)))
                {
                    return null;
                }

                if (_messageQueue.TryDequeue(out var result))
                {
                    return result;
                }

                return null;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception)
            {
                // handle or log as needed
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
                this._processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }

            this._processor.ProcessMessageAsync -= this.ProcessMessageHandler;
            this._processor.ProcessErrorAsync -= this.ProcessErrorHandler;

            this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();

            _messageAvailable.Dispose();

            _disposed = true;
        }
    }
}
