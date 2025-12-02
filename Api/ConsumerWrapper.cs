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
        private readonly ServiceBusProcessor _processor;
        private readonly SemaphoreSlim _messageAvailable = new SemaphoreSlim(0);
        private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));
            var opts = options ?? new ServiceBusProcessorOptions();

            // Create the processor for the topic
            this._processor = client.CreateProcessor(this._topicName, opts);

            // Register message and error handlers
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
                _messageQueue.Enqueue(body);
                _messageAvailable.Release();
                // If auto-complete is disabled, you could call args.CompleteMessageAsync(args.Message)
                // but we avoid interfering with the processor's auto-complete behavior here.
            }
            catch
            {
                // swallow to avoid crashing the processor loop
            }
            await Task.CompletedTask;
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // log or handle the error as needed
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
                if (!_messageAvailable.Wait(TimeSpan.FromSeconds(1))) return null;

                if (_messageQueue.TryDequeue(out var message))
                {
                    return message;
                }
                return null;
            }
            catch (OperationCanceledException)
            {
                // processing was cancelled/closed - treat as no message
                return null;
            }
            catch (Exception)
            {
                // log or handle as needed
                return null;
            }
        }

        /// <summary>
        /// Properly stop and dispose the processor.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                _processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }
            _processor.ProcessMessageAsync -= MessageHandler;
            _processor.ProcessErrorAsync -= ErrorHandler;
            _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
