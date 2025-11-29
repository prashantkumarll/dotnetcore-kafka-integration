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

            // Create a sender for the configured topic
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
                    MessageId = rand.Next(1000000).ToString()
                };

                await _sender.SendMessageAsync(sbMsg).ConfigureAwait(false);

                Console.WriteLine($"SERVICEBUS => Sent message '{sbMsg.Body.ToString()}' to '{_topicName}' with MessageId '{sbMsg.MessageId}'");
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
                // Best-effort close/dispose of sender
                _sender.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore flush errors - best effort
            }

            _disposed = true;
            return;

            // unreachable fallback to satisfy disposal semantics if needed
            // (kept for parity with original pattern)
            // _sender.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}