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
        private readonly ServiceBusProcessor _processor;
        private readonly BlockingCollection<string> _messageQueue = new BlockingCollection<string>();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Create the processor for the given topic/entity
            this._processor = client.CreateProcessor(this._topicName, options ?? new ServiceBusProcessorOptions());

            // Register handlers
            this._processor.ProcessMessageAsync += ProcessMessageHandler;
            this._processor.ProcessErrorAsync += ProcessErrorHandler;

            // Start the processor (synchronously wait for startup)
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            try
            {
                // Try to take a message from the internal queue with a timeout
                if (_messageQueue.TryTake(out var message, (int)TimeSpan.FromSeconds(1).TotalMilliseconds))
                {
                    return message;
                }
                return null;
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

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();
                // Enqueue the message for synchronous read
                _messageQueue.Add(body);

                // Complete the message
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            }
            catch (Exception)
            {
                // ignore or log
            }
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            // handle errors (log, etc.)
            return Task.CompletedTask;
        }

        /// <summary>
        /// Properly stop and dispose the processor.
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
            this._processor.ProcessMessageAsync -= ProcessMessageHandler;
            this._processor.ProcessErrorAsync -= ProcessErrorHandler;
            this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
