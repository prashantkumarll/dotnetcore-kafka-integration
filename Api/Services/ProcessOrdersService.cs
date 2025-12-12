using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Api.Models;

namespace Api.Services
{
    public class ProcessOrdersService : BackgroundService, IAsyncDisposable, IDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly ServiceBusSender _sender;
        private readonly string _inputQueueName = "orderrequests";
        private readonly string _outputQueueName = "readytoship";

        public ProcessOrdersService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
            
            _client = new ServiceBusClient(connectionString);
            _processor = _client.CreateProcessor(_inputQueueName);
            _sender = _client.CreateSender(_outputQueueName);

            // Configure message handler
            _processor.ProcessMessageAsync += HandleMessageAsync;
            _processor.ProcessErrorAsync += ErrorHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("OrderProcessing Service Started");
            
            await _processor.StartProcessingAsync(stoppingToken);

            try
            {
                // Keep the service running
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when service is stopping
                await _processor.StopProcessingAsync();
            }
        }

        private async Task HandleMessageAsync(ProcessMessageEventArgs args)
        {
            try 
            {
                // Get message body
                string orderRequest = args.Message.Body.ToString();

                // Check if message is null or empty before deserializing
                if (string.IsNullOrWhiteSpace(orderRequest))
                {
                    await args.AbandonMessageAsync(args.Message);
                    return;
                }

                // Deserialize 
                OrderRequest? order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);
                
                if (order == null)
                {
                    Console.WriteLine("Warning: Failed to deserialize order request");
                    await args.DeadLetterMessageAsync(args.Message);
                    return;
                }

                // Process Order
                Console.WriteLine($"Info: OrderHandler => Processing the order for {order.productname}");
                order.status = OrderStatus.COMPLETED;

                // Send to ReadyToShip Queue
                var message = new ServiceBusMessage(JsonConvert.SerializeObject(order));
                await _sender.SendMessageAsync(message);

                // Complete the message
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                await args.AbandonMessageAsync(args.Message);
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Error: {args.Exception.Message}");
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            await _processor.DisposeAsync();
            await _client.DisposeAsync();
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
    }
}