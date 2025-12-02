namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusProcessor _processor;
        private TaskCompletionSource<string> _tcs;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));
            options = options ?? new ServiceBusProcessorOptions();

            // Create the processor for the topic/queue name
            this._processor = client.CreateProcessor(this._topicName, options);

            // Register message and error handlers
            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;

            // Start processing messages
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            var tcs = Interlocked.Exchange(ref _tcs, null);
            if (tcs != null)
            {
                tcs.TrySetResult(body);
            }

            // Complete the message so it is not received again.
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // If an error occurs, satisfy any waiting readMessage with null.
            var tcs = Interlocked.Exchange(ref _tcs, null);
            if (tcs != null)
            {
                tcs.TrySetResult(null);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ConsumerWrapper));
            _tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
            {
                try
                {
                    using (cts.Token.Register(() => _tcs.TrySetResult(null)))
                    {
                        var result = await _tcs.Task;
                        return result;
                    }
                }
                catch (OperationCanceledException)
                {
                    // treat as no message
                    return null;
                }
                finally
                {
                    _tcs = null;
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                _processor.ProcessMessageAsync -= MessageHandler;
                _processor.ProcessErrorAsync -= ErrorHandler;
                _processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }
            _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
