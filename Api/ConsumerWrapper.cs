namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable, IAsyncDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusProcessor _processor;
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Create the ServiceBusProcessor from the provided client.
            this._processor = client.CreateProcessor(this._topicName, options ?? new ServiceBusProcessorOptions());
        }

        /// <summary>
        /// Read a single message, waits up to 1 second. Returns null if no message was available.
        /// </summary>
        public async Task<string> readMessage()
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            Func<ProcessMessageEventArgs, Task> messageHandler = async args =>
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    // Complete the message so it won't be received again.
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                    tcs.TrySetResult(body);
                }
                catch
                {
                    // Preserve error handling strategy - return null to caller
                    tcs.TrySetResult(null);
                }
            };

            Func<ProcessErrorEventArgs, Task> errorHandler = args =>
            {
                // On errors, treat as no message available and allow caller to decide
                tcs.TrySetResult(null);
                return Task.CompletedTask;
            };

            // Register handlers
            _processor.ProcessMessageAsync += messageHandler;
            _processor.ProcessErrorAsync += errorHandler;

            try
            {
                await _processor.StartProcessingAsync().ConfigureAwait(false);

                var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).ConfigureAwait(false);

                if (completedTask == tcs.Task)
                {
                    return await tcs.Task.ConfigureAwait(false);
                }
                else
                {
                    return null;
                }
            }
            catch (TaskCanceledException)
            {
                return null;
            }
            finally
            {
                try
                {
                    await _processor.StopProcessingAsync().ConfigureAwait(false);
                }
                catch
                {
                    // ignore
                }

                // Unregister handlers
                _processor.ProcessMessageAsync -= messageHandler;
                _processor.ProcessErrorAsync -= errorHandler;
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
                _processor?.StopProcessingAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore errors on stop
            }

            try
            {
                _processor?.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch
            {
                // ignore
            }

            _disposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;
            try
            {
                if (_processor != null)
                {
                    await _processor.StopProcessingAsync().ConfigureAwait(false);
                    await _processor.DisposeAsync().ConfigureAwait(false);
                }
            }
            catch
            {
                // ignore
            }
            _disposed = true;
        }
    }
}
