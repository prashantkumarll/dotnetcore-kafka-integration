namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _consumerConfig;
        private readonly IServiceBusProcessor _consumer;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient config, string topicName)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._consumerConfig = config ?? throw new ArgumentNullException(nameof(config));

            // Build the IConsumer instance from the builder
            this._consumer = new ConsumerBuilder<string, string>(this._consumerConfig).Build();

            // Subscribe to the single topic name
            this._consumer.Subscribe(this._topicName);
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
                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));
                if (consumeResult == null) return null;

                // New API exposes Message.Value
                return consumeResult.Message?.Value;
            }
            catch (OperationCanceledException)
            {
                // consumer was cancelled/closed - treat as no message
                return null;
            }
            catch (ConsumeException cex)
            {
                // log or rethrow depending on your logging strategy
                // throw; // uncomment if you want to bubble up
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
                // Attempt to leave the group cleanly
                _consumer.Close();
            }
            catch
            {
                // ignore errors on close
            }
            _consumer.Dispose();
            _disposed = true;
        }
    }
}
