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

            // Create a sender for the given topic
            _sender = client.CreateSender(topicName);
        }

        /// <summary>
        /// Writes a message to the configured topic. Logs delivery info to console.
        /// </summary>
        public async Task writeMessage(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            try
            {
                var sbMessage = new ServiceBusMessage(message)
                {
                    MessageId = rand.Next(5).ToString()
                };

                // Send the message to Azure Service Bus
                await _sender.SendMessageAsync(sbMessage).ConfigureAwait(false);

                Console.WriteLine($"ASB => Sent '{message}' to '{_topicName}'");
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
                // Dispose sender (async) in a blocking manner to ensure in-flight operations complete
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
