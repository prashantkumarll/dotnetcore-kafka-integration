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
        private static readonly Random rand = new Random();
        private bool _disposed = false;
        private TaskCompletionSource<string> _messageTcs;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            var actualOptions = options ?? new ServiceBusProcessorOptions();

            // Create the processor for the topic
            this._processor = this._client.CreateProcessor(this._topicName, actualOptions);

            // Register handlers
            this._processor.ProcessMessageAsync += ProcessMessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;

            // Start processing synchronously during construction
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();
                var tcs = _messageTcs;
                if (tcs != null && !tcs.Task.IsCompleted)
                {
                    // Try to set the result; ignore if already set
                    tcs.TrySetResult(body);
                }
                // Attempt to complete the message if possible
                try
                {
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                }
                catch
                {
                    // ignore completion exceptions to mimic original behavior
                }
            }
            catch
            {
                // swallow to allow processor to continue
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // Add logging as needed. For now, mimic original behavior and swallow.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            _messageTcs = tcs;

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
            {
                try
                {
                    using (cts.Token.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: false))
                    {
                        return await tcs.Task.ConfigureAwait(false);
                    }
                }
                catch (TaskCanceledException)
                {
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    // clear tcs to avoid capturing stale reference
                    if (ReferenceEquals(_messageTcs, tcs))
                    {
                        _messageTcs = null;
                    }
                }
            }
        }

        /// <summary>
        /// Properly close and dispose the consumer.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                // Stop processing
                this._processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }
            try
            {
                this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore dispose errors
            }
            _disposed = true;
        }
    }
}
