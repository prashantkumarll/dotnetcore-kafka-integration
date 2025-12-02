namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));
            var processorOptions = options ?? new ServiceBusProcessorOptions();

            // Create the processor for the given topic (subscription must be part of topicName if needed)
            this._processor = this._client.CreateProcessor(this._topicName, processorOptions);
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            async Task messageHandler(ProcessMessageEventArgs args)
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                    tcs.TrySetResult(body);
                }
                catch
                {
                    tcs.TrySetResult(null);
                }
            }

            Task errorHandler(ProcessErrorEventArgs args)
            {
                // log or handle errors as needed. For single-read we ignore and treat as no message.
                return Task.CompletedTask;
            }

            this._processor.ProcessMessageAsync += messageHandler;
            this._processor.ProcessErrorAsync += errorHandler;

            try
            {
                await this._processor.StartProcessingAsync().ConfigureAwait(false);

                var delayTask = Task.Delay(TimeSpan.FromSeconds(1));
                var completed = await Task.WhenAny(tcs.Task, delayTask).ConfigureAwait(false);

                if (completed == tcs.Task)
                {
                    return await tcs.Task.ConfigureAwait(false);
                }

                return null;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            finally
            {
                try
                {
                    await this._processor.StopProcessingAsync().ConfigureAwait(false);
                }
                catch
                {
                    // ignore
                }

                this._processor.ProcessMessageAsync -= messageHandler;
                this._processor.ProcessErrorAsync -= errorHandler;
            }
        }

        /// <summary>
        /// Properly close and dispose the processor.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                // Attempt to stop processing gracefully
                this._processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }
            this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
