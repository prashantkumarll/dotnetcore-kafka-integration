namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ProducerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusSender _sender;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ProducerWrapper(ServiceBusClient client, string topicName)
        {
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            client = client ?? throw new ArgumentNullException(nameof(client));

            // Create a ServiceBusSender for the specified topic
            _sender = client.CreateSender(_topicName);
        }

        /// <summary>
        /// Writes a message to the configured topic. Logs delivery info to console.
        /// </summary>
        public async Task writeMessage(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            try
            {
                var sbMsg = new ServiceBusMessage(new BinaryData(message))
                {
                    MessageId = rand.Next(5).ToString()
                };

                await _sender.SendMessageAsync(sbMsg).ConfigureAwait(false);

                Console.WriteLine($"SERVICE BUS => Sent '{message}' to '{_topicName}' with MessageId '{sbMsg.MessageId}'");
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
                // Best effort to dispose sender - use DisposeAsync synchronously
                _sender.DisposeAsync().AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // ignore flush errors - best effort
            }
            _disposed = true;
        }
    }
}
