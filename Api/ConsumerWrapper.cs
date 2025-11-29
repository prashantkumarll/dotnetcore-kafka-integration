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
        private TaskCompletionSource<string> _tcs;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            this._client = client ?? throw new ArgumentNullException(nameof(client));

            options = options ?? new ServiceBusProcessorOptions();

            // Create the ServiceBusProcessor for the topic/subscription
            this._processor = this._client.CreateProcessor(this._topicName, options);

            // Register handlers
            this._processor.ProcessMessageAsync += this.MessageHandler;
            this._processor.ProcessErrorAsync += this.ErrorHandler;
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();
                var tcs = Interlocked.Exchange(ref _tcs, null);
                if (tcs != null)
                {
                    tcs.TrySetResult(body);
                }
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            }
            catch
            {
                // ignore handler exceptions
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // Log or handle error as needed
            return Task.CompletedTask;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            var localTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (Interlocked.CompareExchange(ref _tcs, localTcs, null) != null)
            {
                throw new InvalidOperationException("A read is already in progress.");
            }

            try
            {
                // Start processing
                this._processor.StartProcessingAsync().GetAwaiter().GetResult();

                var task = localTcs.Task;
                if (task.Wait(TimeSpan.FromSeconds(1)))
                {
                    return task.Result;
                }
                else
                {
                    // timeout - no message
                    Interlocked.CompareExchange(ref _tcs, null, localTcs);
                    return null;
                }
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
                    this._processor.StopProcessingAsync().GetAwaiter().GetResult();
                }
                catch
                {
                    // ignore errors on stop
                }
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
                try
                {
                    this._processor.StopProcessingAsync().GetAwaiter().GetResult();
                }
                catch
                {
                }

                this._processor.ProcessMessageAsync -= this.MessageHandler;
                this._processor.ProcessErrorAsync -= this.ErrorHandler;
                this._processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on dispose
            }

            _disposed = true;
        }
    }
}
