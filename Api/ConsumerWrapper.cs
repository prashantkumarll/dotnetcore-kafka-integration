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

            var opts = options ?? new ServiceBusProcessorOptions();

            // Create the ServiceBusProcessor for the given topic/queue name.
            this._processor = this._client.CreateProcessor(this._topicName, opts);
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            async Task ProcessMessageHandler(ProcessMessageEventArgs args)
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    // Complete the message so it won't be received again.
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                    tcs.TrySetResult(body);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Task ProcessErrorHandler(ProcessErrorEventArgs args)
            {
                // Log the error as needed. For now, ignore and continue.
                return Task.CompletedTask;
            }

            // Register handlers
            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ProcessErrorHandler;

            try
            {
                await _processor.StartProcessingAsync().ConfigureAwait(false);

                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                {
                    try
                    {
                        using (cts.Token.Register(() => tcs.TrySetResult(null)))
                        {
                            var result = await tcs.Task.ConfigureAwait(false);
                            return result;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        return null;
                    }
                    catch (Exception)
                    {
                        // treat exceptions similar to Kafka ConsumeException
                        return null;
                    }
                }
            }
            finally
            {
                try
                {
                    await _processor.StopProcessingAsync().ConfigureAwait(false);
                }
                catch
                {
                    // ignore errors on stop
                }

                // Unregister handlers
                _processor.ProcessMessageAsync -= ProcessMessageHandler;
                _processor.ProcessErrorAsync -= ProcessErrorHandler;
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
                // Attempt to stop processing cleanly
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
