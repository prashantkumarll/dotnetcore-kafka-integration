namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IAsyncDisposable, IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            // Create the processor for the given topic
            this._processor = this._client.CreateProcessor(this._topicName, options ?? new ServiceBusProcessorOptions());
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            // Handler for processing messages
            async Task ProcessMessageHandler(ProcessMessageEventArgs args)
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    await args.CompleteMessageAsync(args.Message);
                    tcs.TrySetResult(body);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Task ProcessErrorHandler(ProcessErrorEventArgs args)
            {
                // Log or ignore - surface no message to caller
                return Task.CompletedTask;
            }

            this._processor.ProcessMessageAsync += ProcessMessageHandler;
            this._processor.ProcessErrorAsync += ProcessErrorHandler;

            try
            {
                await this._processor.StartProcessingAsync();

                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                {
                    try
                    {
                        return await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token)) == tcs.Task
                            ? await tcs.Task
                            : null;
                    }
                    catch (OperationCanceledException)
                    {
                        return null;
                    }
                }
            }
            finally
            {
                // Stop processing and unregister handlers
                try
                {
                    await this._processor.StopProcessingAsync();
                }
                catch
                {
                    // ignore
                }
                this._processor.ProcessMessageAsync -= ProcessMessageHandler;
                this._processor.ProcessErrorAsync -= ProcessErrorHandler;
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
                this._processor.CloseAsync().GetAwaiter().GetResult();
            }
            catch
            {
            }
            this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;
            try
            {
                await this._processor.StopProcessingAsync();
            }
            catch
            {
            }
            await this._processor.DisposeAsync();
            _disposed = true;
        }
    }
}
