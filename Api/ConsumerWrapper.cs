namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IAsyncDisposable, IDisposable
    {
        private readonly string _topicName;
        private readonly string _connectionString;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusReceiver _receiver;
        private bool _disposed = false;

        public ConsumerWrapper(string connectionString, string topicName)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            // Create ServiceBus client and receiver
            this._client = new ServiceBusClient(_connectionString);
            this._receiver = _client.CreateReceiver(_topicName);
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> ReadMessageAsync()
        {
            try
            {
                // Use ReceiveMessageAsync with a timeout
                var message = await _receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(1));
                
                if (message == null) return null;

                // Convert message body to string
                string messageBody = message.Body.ToString();

                // Complete the message to remove it from the queue
                await _receiver.CompleteMessageAsync(message);

                return messageBody;
            }
            catch (Exception)
            {
                // Log or handle specific exceptions as needed
                return null;
            }
        }

        /// <summary>
        /// Async disposal method
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            await _receiver.DisposeAsync();
            await _client.DisposeAsync();

            _disposed = true;
        }

        /// <summary>
        /// Synchronous disposal method for compatibility
        /// </summary>
        public void Dispose()
        {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}