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

            // Use ServiceBusClient to create a sender for the topic/queue
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
                var sbMsg = new ServiceBusMessage(message)
                {
                    MessageId = rand.Next(5).ToString()
                };

                // SendMessageAsync sends the message to Service Bus
                await _sender.SendMessageAsync(sbMsg).ConfigureAwait(false);

                Console.WriteLine($"SERVICE BUS => Sent '{message}' to '{_topicName}' with MessageId '{sbMsg.MessageId}'");
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
                // Close sender (best effort)
                _sender.CloseAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore flush errors - best effort
            }

            _sender.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
