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
        private volatile TaskCompletionSource<string> _tcs;
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));
            options = options ?? new ServiceBusProcessorOptions();

            this._tcs = null;

            // Create processor for the topic
            this._processor = client.CreateProcessor(this._topicName, options);

            // Register handlers
            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;

            // Start processing
            this._processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        public async Task<string> readMessage()
        {
            // Set up a TCS that will be completed by the message handler
            var newTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            // Ensure that only one waiter is active at a time
            if (Interlocked.CompareExchange(ref _tcs, newTcs, null) != null)
            {
                // Another waiter exists, replace it
                // For simplicity, return null
                return null;
            }

            try
            {
                // Wait up to 1 second for a message
                var completed = await Task.WhenAny(newTcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).ConfigureAwait(false);
                if (completed == newTcs.Task)
                {
                    return await newTcs.Task.ConfigureAwait(false);
                }
                return null;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            finally
            {
                // Clear the TCS so future calls can await
                Interlocked.Exchange(ref _tcs, null);
            }
        }

        private Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();

                var tcs = Interlocked.Exchange(ref _tcs, null);
                if (tcs != null)
                {
                    // Try to set result without throwing if already completed
                    tcs.TrySetResult(body);
                }

                // Complete the message so it won't be received again
                return args.CompleteMessageAsync(args.Message);
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // Optionally log the error
            return Task.CompletedTask;
        }

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
