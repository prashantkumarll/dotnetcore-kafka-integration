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
        private readonly ServiceBusClient _client;
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));

            var processorOptions = options ?? new ServiceBusProcessorOptions();
            this._processor = this._client.CreateProcessor(this._topicName, processorOptions);
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            Task ProcessMessageHandler(ProcessMessageEventArgs args)
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    // Complete the message so it's removed from the queue/topic subscription
                    var completeTask = args.CompleteMessageAsync(args.Message);
                    // Continue after completing to set result
                    completeTask.ContinueWith(t =>
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            tcs.TrySetResult(body);
                        }
                        else
                        {
                            tcs.TrySetResult(null);
                        }
                    }, TaskScheduler.Default);
                }
                catch
                {
                    tcs.TrySetResult(null);
                }
                return Task.CompletedTask;
            }

            Task ProcessErrorHandler(ProcessErrorEventArgs args)
            {
                // Log or inspect args.Exception as needed. For now, treat as non-fatal.
                return Task.CompletedTask;
            }

            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ProcessErrorHandler;

            try
            {
                await _processor.StartProcessingAsync();

                var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(1)));
                if (completedTask == tcs.Task)
                {
                    return await tcs.Task;
                }

                // timeout
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
                    await _processor.StopProcessingAsync();
                }
                catch
                {
                    // ignore
                }

                _processor.ProcessMessageAsync -= ProcessMessageHandler;
                _processor.ProcessErrorAsync -= ProcessErrorHandler;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                try
                {
                    _processor.StopProcessingAsync().GetAwaiter().GetResult();
                }
                catch
                {
                    // ignore errors on stop
                }

                _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore
            }

            _disposed = true;
        }
    }
}
