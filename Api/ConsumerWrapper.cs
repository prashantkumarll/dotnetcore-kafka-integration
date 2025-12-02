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
        private TaskCompletionSource<string> _tcs;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            var opts = options ?? new ServiceBusProcessorOptions();

            // Create the processor for the specified entity
            this._processor = this._client.CreateProcessor(this._topicName, opts);

            // Register handlers
            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;

            // Start the processor synchronously from the constructor
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            try
            {
                // Prepare a TaskCompletionSource which will be completed by the message handler.
                _tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

                // Wait up to one second for a message to arrive.
                var task = _tcs.Task;
                bool completed = task.Wait(TimeSpan.FromSeconds(1));
                if (!completed) return null;

                return task.Result;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception)
            {
                // log or rethrow depending on your logging strategy
                return null;
            }
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();

                // Try to complete the waiting TaskCompletionSource if present.
                var tcs = _tcs;
                if (tcs != null && !tcs.Task.IsCompleted)
                {
                    tcs.TrySetResult(body);
                }

                // Optionally complete the message if auto-complete is disabled.
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            }
            catch
            {
                // ignore processing errors for now
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // Optionally log the error. For now, just ignore.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Properly close and dispose the processor.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                this._processor.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }

            this._processor.ProcessMessageAsync -= MessageHandler;
            this._processor.ProcessErrorAsync -= ErrorHandler;

            this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();

            _disposed = true;
        }
    }
}
