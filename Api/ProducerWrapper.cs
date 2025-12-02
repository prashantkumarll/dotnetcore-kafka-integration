namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ProducerWrapper : IDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly string _topicName;
        private readonly ServiceBusSender _sender;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ProducerWrapper(ServiceBusClient client, string topicName)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));

            // Create a sender for the given topic
            _sender = _client.CreateSender(_topicName);
        }

        /// <summary>
        /// Writes a message to the configured topic. Logs send info to console.
        /// </summary>
        public async Task writeMessage(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            try
            {
                var sbMessage = new ServiceBusMessage(BinaryData.FromString(message))
                {
                    MessageId = rand.Next().ToString()
                };
                // attach a key similar to Kafka key
                sbMessage.ApplicationProperties["key"] = rand.Next(5).ToString();

                await _sender.SendMessageAsync(sbMessage).ConfigureAwait(false);

                Console.WriteLine($"SERVICEBUS => Sent MessageId '{sbMessage.MessageId}' to '{_topicName}'");
            }
            catch (ServiceBusException sbex)
            {
                Console.WriteLine($"Send failed: {sbex.Reason}");
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                // Best-effort to close sender synchronously
                _sender.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore dispose errors
            }

            _disposed = true;
        }
    }
}
