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
    public class ProcessOrdersService : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly ServiceBusSender _sender;
        private readonly IConfiguration _configuration;

        public ProcessOrdersService(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("ServiceBusConnectionString");
            
            _client = new ServiceBusClient(connectionString);
            _processor = _client.CreateProcessor("orderrequests");
            _sender = _client.CreateSender("readytoship");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("OrderProcessing Service Started");

            // Configure message handler
            _processor.ProcessMessageAsync += HandleMessageAsync;
            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync(stoppingToken);

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            await _processor.StopProcessingAsync(stoppingToken);
        }

        private async Task HandleMessageAsync(ProcessMessageEventArgs args)
        {
            try 
            {
                string orderRequest = args.Message.Body.ToString();

                // Check if message is null or empty before deserializing
                if (string.IsNullOrWhiteSpace(orderRequest))
                {
                    await args.CompleteMessageAsync(args.Message);
                    return;
                }

                // Deserialize 
                OrderRequest? order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);
                
                if (order == null)
                {
                    Console.WriteLine("Warning: Failed to deserialize order request");
                    await args.CompleteMessageAsync(args.Message);
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
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Error: {args.Exception.Message}");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _client.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}