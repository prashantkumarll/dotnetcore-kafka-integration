namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ProducerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusSender _producer;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ProducerWrapper(ServiceBusClient client, string topicName)
        {
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Create a ServiceBus sender for the given topic
            _producer = client.CreateSender(_topicName);

            // No direct equivalent of SetErrorHandler; errors are surfaced from SendMessageAsync
        }

        /// <summary>
        /// Writes a message to the configured topic. Logs delivery info to console.
        /// </summary>
        public async Task writeMessage(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            try
            {
                var sbMsg = new ServiceBusMessage(message)
                {
                    MessageId = rand.Next(5).ToString()
                };

                await _producer.SendMessageAsync(sbMsg).ConfigureAwait(false);

                Console.WriteLine($"SERVICE BUS => Sent '{message}' to '{_topicName}'");
            }
            catch (Exception ex)
            {
                // You can log more details or rethrow based on your strategy
                Console.WriteLine($"Send failed: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                // Ensure sender is closed/disposed
                _producer.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore dispose errors - best effort
            }

            _disposed = true;
        }
    }
}
