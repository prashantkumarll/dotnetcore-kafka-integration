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
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            var procOptions = options ?? new ServiceBusProcessorOptions();
            this._processor = this._client.CreateProcessor(this._topicName, procOptions);

            // register handlers; processing will be started when reading
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            // Use a short timeout so this method doesn't block indefinitely.
            // You can adjust the timeout or add an overload that accepts CancellationToken.
            this._tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            await _processor.StartProcessingAsync();

            try
            {
                var delayTask = Task.Delay(TimeSpan.FromSeconds(1));
                var completed = await Task.WhenAny(this._tcs.Task, delayTask);
                if (completed == this._tcs.Task)
                {
                    return await this._tcs.Task;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                try
                {
                    await _processor.StopProcessingAsync();
                }
                catch
                {
                    // ignore stop errors
                }
                // clear the completion source
                Interlocked.Exchange(ref _tcs, null);
            }
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();
                var tcs = Interlocked.Exchange(ref _tcs, null);
                if (tcs != null)
                {
                    tcs.TrySetResult(body);
                }
                await args.CompleteMessageAsync(args.Message);
            }
            catch
            {
                // ignore handler exceptions
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // you could log args.Exception etc.
            var tcs = Interlocked.Exchange(ref _tcs, null);
            if (tcs != null)
            {
                tcs.TrySetResult(null);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Properly close and dispose the consumer.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                // Attempt to stop processing
                _processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }
            try
            {
                _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore dispose errors
            }
            _disposed = true;
        }
    }
}
