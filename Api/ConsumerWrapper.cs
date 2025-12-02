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

            var opts = options ?? new ServiceBusProcessorOptions();

            // Create the processor for the given topic (or queue) name
            this._processor = this._client.CreateProcessor(this._topicName, opts);
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
                    // Complete the message so it's removed from the queue/topic
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                    tcs.TrySetResult(body);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                // Propagate error to waiting task
                tcs.TrySetException(args.Exception);
                return Task.CompletedTask;
            }

            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;

            try
            {
                await this._processor.StartProcessingAsync().ConfigureAwait(false);

                var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).ConfigureAwait(false);
                if (completedTask != tcs.Task)
                {
                    // Timeout - no message received
                    return null;
                }

                return await tcs.Task.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception)
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
                this._processor.ProcessMessageAsync -= MessageHandler;
                this._processor.ProcessErrorAsync -= ErrorHandler;
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
                // Attempt to stop processing
                this._processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on close
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
