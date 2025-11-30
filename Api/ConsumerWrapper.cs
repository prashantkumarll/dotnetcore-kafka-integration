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
        private TaskCompletionSource<string> _tcs;
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            var svcClient = client ?? throw new ArgumentNullException(nameof(client));
            options = options ?? new ServiceBusProcessorOptions();

            // Create the processor for the specified topic
            this._processor = svcClient.CreateProcessor(this._topicName, options);

            // Register message and error handlers
            this._processor.ProcessMessageAsync += MessageHandler;
            this._processor.ProcessErrorAsync += ErrorHandler;
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();
                // Try to set the result for any waiting readMessage call
                var tcs = _tcs;
                if (tcs != null)
                {
                    tcs.TrySetResult(body);
                }

                // Complete the message so it's removed from the queue/subscription
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            }
            catch
            {
                // ignore handler exceptions
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // log errors as needed. For now, swallow to mimic previous behavior.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            // Ensure only one waiting TCS at a time
            _tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            try
            {
                await _processor.StartProcessingAsync().ConfigureAwait(false);
            }
            catch
            {
                // ignore start errors
            }

            var completed = await Task.WhenAny(_tcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).ConfigureAwait(false);
            string result = null;
            if (completed == _tcs.Task)
            {
                result = await _tcs.Task.ConfigureAwait(false);
            }

            try
            {
                await _processor.StopProcessingAsync().ConfigureAwait(false);
            }
            catch
            {
                // ignore stop errors
            }

            return result;
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
            _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}
