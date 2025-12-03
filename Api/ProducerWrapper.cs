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
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Create a sender for the given topic (queue/topic) using ServiceBusClient
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
                var msg = new ServiceBusMessage(message)
                {
                    MessageId = rand.Next(5).ToString()
                };

                // Optionally add key as an application property
                msg.ApplicationProperties["Key"] = rand.Next(5).ToString();

                // Send the message asynchronously
                await _sender.SendMessageAsync(msg).ConfigureAwait(false);

                Console.WriteLine($"SERVICEBUS => Sent message to '{_topicName}' with MessageId '{msg.MessageId}'");
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
                // Dispose the sender; use sync wait on the async dispose
                _sender.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore flush errors - best effort
            }
            _disposed = true;
        }
    }
}
