namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private TaskCompletionSource<string> _tcs;
        private readonly object _tcsLock = new object();
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            var opts = options ?? new ServiceBusProcessorOptions();

            // Create the processor for the single topic (subscription handling depends on topology)
            this._processor = this._client.CreateProcessor(this._topicName, opts);

            // Register handlers
            this._processor.ProcessMessageAsync += this.MessageHandler;
            this._processor.ProcessErrorAsync += this.ErrorHandler;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ConsumerWrapper));

            lock (_tcsLock)
            {
                // create a fresh TCS for this read attempt
                _tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            }

            // Ensure the processor is started. Start synchronously to keep API surface simple.
            try
            {
                this._processor.StartProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // If processor cannot be started treat as no message available
                return null;
            }

            try
            {
                // Wait up to 1 second for a message to arrive
                var completed = _tcs.Task.Wait(TimeSpan.FromSeconds(1));
                if (!completed) return null;

                return _tcs.Task.Result;
            }
            finally
            {
                // Clear the tcs so subsequent reads create a fresh one
                lock (_tcsLock)
                {
                    _tcs = null;
                }
            }
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message?.Body.ToString();

            // If a reader is waiting, complete its task
            TaskCompletionSource<string> tcsCopy = null;
            lock (_tcsLock)
            {
                tcsCopy = _tcs;
            }

            if (tcsCopy != null && !tcsCopy.Task.IsCompleted)
            {
                tcsCopy.TrySetResult(body);
            }

            // Complete the message so it's removed from the queue
            try
            {
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            }
            catch
            {
                // swallow any errors completing the message - readMessage has received the payload
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // For this wrapper we ignore errors and allow readMessage to return null on timeouts.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Properly close and dispose the processor.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                this._processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors stopping the processor
            }

            // Unregister handlers and dispose
            try
            {
                this._processor.ProcessMessageAsync -= this.MessageHandler;
                this._processor.ProcessErrorAsync -= this.ErrorHandler;
                this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore
            }

            _disposed = true;
        }
    }
}
