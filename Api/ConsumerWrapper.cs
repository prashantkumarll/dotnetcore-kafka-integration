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
        private TaskCompletionSource<string> _messageTcs;
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            var opts = options ?? new ServiceBusProcessorOptions();
            this._processor = this._client.CreateProcessor(this._topicName, opts);

            // Attach handlers; processing is started on-demand to keep constructor synchronous.
            this._processor.ProcessMessageAsync += ProcessMessageHandler;
            this._processor.ProcessErrorAsync += ProcessErrorHandler;
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message?.Body.ToString();

            var tcs = Interlocked.Exchange(ref _messageTcs, null);
            if (tcs != null)
            {
                tcs.TrySetResult(body);
            }

            try
            {
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            }
            catch
            {
                // ignore errors completing the message
            }
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            // If an error occurs, ensure any pending reader is notified with null.
            var tcs = Interlocked.Exchange(ref _messageTcs, null);
            if (tcs != null)
            {
                tcs.TrySetResult(null);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ConsumerWrapper));

            var newTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (Interlocked.CompareExchange(ref _messageTcs, newTcs, null) != null)
            {
                throw new InvalidOperationException("A read is already in progress.");
            }

            // Start processing if not already started.
            _processor.StartProcessingAsync().GetAwaiter().GetResult();

            var completed = Task.WhenAny(newTcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).GetAwaiter().GetResult();
            if (completed == newTcs.Task)
            {
                return newTcs.Task.GetAwaiter().GetResult();
            }

            // timeout - clean up pending tcs if still set
            Interlocked.CompareExchange(ref _messageTcs, null, newTcs);
            return null;
        }

        /// <summary>
        /// Properly close and dispose the consumer.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                _processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }

            // Detach handlers and dispose processor
            try
            {
                this._processor.ProcessMessageAsync -= ProcessMessageHandler;
                this._processor.ProcessErrorAsync -= ProcessErrorHandler;
            }
            catch
            {
                // ignore
            }

            this._processor.DisposeAsync().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
