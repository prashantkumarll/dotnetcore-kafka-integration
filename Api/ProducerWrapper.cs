namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ProducerWrapper : IAsyncDisposable, IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ProducerWrapper(string connectionString, string topicName)
        {
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            
            // Create ServiceBusClient with connection string
            _client = new ServiceBusClient(connectionString);
            
            // Create a sender for the specific topic
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
                // Create ServiceBus message
                var serviceBusMessage = new ServiceBusMessage(message)
                {
                    MessageId = rand.Next(5).ToString()
                };

                // Send message
                await _sender.SendMessageAsync(serviceBusMessage).ConfigureAwait(false);

                Console.WriteLine($"SERVICE BUS => Delivered '{message}' to '{_topicName}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send failed: {ex.Message}");
                throw;
            }
        }

        // Async disposal method
        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            await _sender.DisposeAsync();
            await _client.DisposeAsync();

            _disposed = true;
        }

        // Synchronous disposal method for compatibility
        public void Dispose()
        {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}