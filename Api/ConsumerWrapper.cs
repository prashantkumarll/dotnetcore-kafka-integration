namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusProcessorOptions _processorOptions;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));
            this._processorOptions = options ?? new ServiceBusProcessorOptions();

            // Build the ServiceBusProcessor instance from the client
            this._processor = this._client.CreateProcessor(this._topicName, this._processorOptions);

            // Handlers are registered per read to support synchronous-like reads.
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ConsumerWrapper));

            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    await args.CompleteMessageAsync(args.Message);
                    tcs.TrySetResult(body);
                }
                catch (OperationCanceledException)
                {
                    // treat as no message
                    tcs.TrySetResult(null);
                }
                catch (Exception)
                {
                    // log as needed; treat as no message
                    tcs.TrySetResult(null);
                }
            }

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                // log error if desired
                return Task.CompletedTask;
            }

            try
            {
                _processor.ProcessMessageAsync += MessageHandler;
                _processor.ProcessErrorAsync += ErrorHandler;

                await _processor.StartProcessingAsync();

                var delayTask = Task.Delay(TimeSpan.FromSeconds(1));
                var completed = await Task.WhenAny(tcs.Task, delayTask);
                string result = null;
                if (completed == tcs.Task)
                {
                    result = await tcs.Task;
                }

                await _processor.StopProcessingAsync();

                return result;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception)
            {
                // log or rethrow depending on strategy
                return null;
            }
            finally
            {
                // Ensure handlers are removed
                _processor.ProcessMessageAsync -= MessageHandler;
                _processor.ProcessErrorAsync -= ErrorHandler;
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
