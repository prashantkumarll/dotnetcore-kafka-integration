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

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            // Create the ServiceBusProcessor instance from the client
            this._processor = this._client.CreateProcessor(this._topicName, options ?? new ServiceBusProcessorOptions());
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                    tcs.TrySetResult(body);
                }
                catch (Exception)
                {
                    tcs.TrySetResult(null);
                }
            }

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                // log or handle error as needed; treat as no message available for this call
                tcs.TrySetResult(null);
                return Task.CompletedTask;
            }

            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;

            try
            {
                await this._processor.StartProcessingAsync().ConfigureAwait(false);

                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                {
                    try
                    {
                        var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token)).ConfigureAwait(false);
                        if (completedTask == tcs.Task)
                        {
                            return await tcs.Task.ConfigureAwait(false);
                        }

                        return null;
                    }
                    catch (OperationCanceledException)
                    {
                        // timeout expired
                        return null;
                    }
                }
            }
            finally
            {
                try
                {
                    await this._processor.StopProcessingAsync().ConfigureAwait(false);
                }
                catch
                {
                    // ignore stop errors
                }

                this._processor.ProcessMessageAsync -= MessageHandler;
                this._processor.ProcessErrorAsync -= ErrorHandler;
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
                // Attempt to stop processing
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
