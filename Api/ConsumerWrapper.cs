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
            options = options ?? new ServiceBusProcessorOptions();

            // Create the ServiceBusProcessor for the provided topic/queue name
            this._processor = this._client.CreateProcessor(this._topicName, options);
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public string readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            async Task ProcessMessageHandler(ProcessMessageEventArgs args)
            {
                try
                {
                    // Convert the message body to string
                    var body = args.Message.Body.ToString();
                    tcs.TrySetResult(body);

                    // Complete the message so it won't be reprocessed
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Task ProcessErrorHandler(ProcessErrorEventArgs args)
            {
                // Surface error to the waiting caller
                tcs.TrySetException(args.Exception);
                return Task.CompletedTask;
            }

            // Register handlers
            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ProcessErrorHandler;

            try
            {
                // Start processing
                _processor.StartProcessingAsync().GetAwaiter().GetResult();

                // Wait up to 1 second for a message
                var completed = tcs.Task.Wait(TimeSpan.FromSeconds(1));
                if (!completed)
                {
                    return null;
                }

                if (tcs.Task.IsFaulted) return null;

                return tcs.Task.Result;
            }
            catch (OperationCanceledException)
            {
                // processing was cancelled - treat as no message
                return null;
            }
            catch (Exception)
            {
                // log or handle as needed; returning null to match previous behavior
                return null;
            }
            finally
            {
                try
                {
                    _processor.StopProcessingAsync().GetAwaiter().GetResult();
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
        /// Properly stop and dispose the processor.
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
                // ignore
            }
            _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
