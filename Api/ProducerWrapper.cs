namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ProducerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ProducerWrapper(ServiceBusClient client, string topicName)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));

            // Create a sender for the topic
            _sender = _client.CreateSender(_topicName);
        }

        /// <summary>
        /// Writes a message to the configured topic. Logs delivery info to console.
        /// </summary>
        public async Task writeMessage(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            try
            {
                var msg = new ServiceBusMessage(message)
                {
                    MessageId = rand.Next(5).ToString()
                };

                await _sender.SendMessageAsync(msg).ConfigureAwait(false);

                Console.WriteLine($"SERVICEBUS => Sent '{message}' to '{_topicName}'");
            }
            catch (ServiceBusException sbex)
            {
                // You can log more details or rethrow based on your strategy
                Console.WriteLine($"Send failed: {sbex.Reason}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send failed: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                // Attempt to dispose the sender (synchronously)
                _sender.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore dispose errors - best effort
            }

            _disposed = true;
        }
    }
}
