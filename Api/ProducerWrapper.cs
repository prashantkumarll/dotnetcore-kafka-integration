namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ProducerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _config;
        private readonly IServiceBusSender _producer;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ProducerWrapper(ServiceBusClient config, string topicName)
        {
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            // Use ProducerBuilder and set an error handler
            _producer = new ProducerBuilder<string, string>(_config)
                            .SetErrorHandler((prod, err) =>
                            {
                                Console.WriteLine($"Producer error: {err.Reason}");
                            })
                            .Build();
        }

        /// <summary>
        /// Writes a message to the configured topic. Logs delivery info to console.
        /// </summary>
        public async Task writeMessage(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            try
            {
                var msg = new ServiceBusMessage
                {
                    Key = rand.Next(5).ToString(),
                    Value = message
                };

                // SendMessageAsync returns DeliveryResult<TKey, TValue>
                var dr = await _producer.SendMessageAsync(_topicName, msg).ConfigureAwait(false);

                // New API exposes the produced message on dr.Message
                Console.WriteLine($"KAFKA => Delivered '{dr.Message?.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<string, string> pex)
            {
                // You can log more details or rethrow based on your strategy
                Console.WriteLine($"Produce failed: {pex.Error.Reason}");
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                // Block until outstanding messages are sent (or timeout)
                _producer.Flush(TimeSpan.FromSeconds(10));
            }
            catch
            {
                // ignore flush errors - best effort
            }

            _producer.Dispose();
            _disposed = true;
        }
    }
}
