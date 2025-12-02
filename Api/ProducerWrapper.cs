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
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            _client = client ?? throw new ArgumentNullException(nameof(client));

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
                var sbMessage = new ServiceBusMessage(message);
                // Optionally set a message id or application properties
                sbMessage.MessageId = rand.Next(10000).ToString();

                await _sender.SendMessageAsync(sbMessage).ConfigureAwait(false);

                Console.WriteLine($"SERVICE BUS => Sent message '{message}' to '{_topicName}' with MessageId '{sbMessage.MessageId}'");
            }
            catch (ServiceBusException sbEx)
            {
                Console.WriteLine($"Send failed: {sbEx.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                // Close the sender to flush outstanding messages
                _sender.CloseAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore close errors - best effort
            }

            _sender.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
