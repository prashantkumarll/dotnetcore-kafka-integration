namespace Api
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class ConsumerWrapper : IDisposable
    {
        private readonly string _topicName;
        private readonly ServiceBusProcessor _processor;
        private static readonly Random rand = new Random();
        private bool _disposed = false;

        public ConsumerWrapper(ServiceBusClient client, string topicName, ServiceBusProcessorOptions options = null)
        {
            this._topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Create the ServiceBusProcessor instance from the client
            this._processor = client.CreateProcessor(this._topicName, options ?? new ServiceBusProcessorOptions());
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
                    tcs.TrySetResult(body);
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Task ProcessErrorHandler(ProcessErrorEventArgs args)
            {
                // Log or handle the error as needed. For this wrapper we treat errors as non-fatal.
                return Task.CompletedTask;
            }

            // Register handlers
            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ProcessErrorHandler;

            try
            {
                await _processor.StartProcessingAsync().ConfigureAwait(false);

                // Wait for a message or timeout after 1 second
                var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(1))).ConfigureAwait(false);
                if (completed == tcs.Task)
                {
                    return await tcs.Task.ConfigureAwait(false);
                }
                return null;
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
