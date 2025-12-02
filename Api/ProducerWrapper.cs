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
            if (client == null) throw new ArgumentNullException(nameof(client));
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));

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
                var key = rand.Next(5).ToString();
                var sbMessage = new ServiceBusMessage(BinaryData.FromString(message))
                {
                    MessageId = Guid.NewGuid().ToString()
                };
                sbMessage.ApplicationProperties["Key"] = key;

                await _sender.SendMessageAsync(sbMessage).ConfigureAwait(false);

                Console.WriteLine($"SB => Sent '{message}' to '{_topicName}' with Key='{key}'");
            }
            catch (ServiceBusException sbex)
            {
                Console.WriteLine($"Send failed: {sbex.Message}");
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
                // Best-effort dispose of sender
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
