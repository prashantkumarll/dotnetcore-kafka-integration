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
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            var processorOptions = options ?? new ServiceBusProcessorOptions();

            // Build the ServiceBusProcessor from the client
            this._processor = this._client.CreateProcessor(this._topicName, processorOptions);
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            Task ProcessMessageAsync(ProcessMessageEventArgs args)
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    // Complete the message so it won't be received again
                    var completeTask = args.CompleteMessageAsync(args.Message);
                    tcs.TrySetResult(body);
                    return completeTask;
                }
                catch
                {
                    tcs.TrySetResult(null);
                    return Task.CompletedTask;
                }
            }

            Task ProcessErrorAsync(ProcessErrorEventArgs args)
            {
                // log or handle the error according to your strategy
                tcs.TrySetResult(null);
                return Task.CompletedTask;
            }

            try
            {
                _processor.ProcessMessageAsync += ProcessMessageAsync;
                _processor.ProcessErrorAsync += ProcessErrorAsync;

                await _processor.StartProcessingAsync();

                var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(1)));
                if (completedTask != tcs.Task)
                {
                    // timed out - treat as no message
                    return null;
                }

                return await tcs.Task;
            }
            catch (OperationCanceledException)
            {
                // processor was cancelled/closed - treat as no message
                return null;
            }
            catch (Exception)
            {
                // log or rethrow depending on your logging strategy
                return null;
            }
            finally
            {
                try
                {
                    await _processor.StopProcessingAsync();
                }
                catch
                {
                    // ignore errors on stop
                }

                _processor.ProcessMessageAsync -= ProcessMessageAsync;
                _processor.ProcessErrorAsync -= ProcessErrorAsync;
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
