namespace Api.Services
{
    using Microsoft.Extensions.Hosting;
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    using Api.Models;
    using Newtonsoft.Json;
    using Azure.Messaging.ServiceBus;

    public class ProcessOrdersService : BackgroundService
    {
        private readonly ServiceBusClient client;
        public ProcessOrdersService(ServiceBusClient client)
        {
            this.client = client;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("OrderProcessing Service Started");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var consumerHelper = new ConsumerWrapper(client, "orderrequests"))
                {
                    string orderRequest = await consumerHelper.readMessage();

                    // Check if message is null or empty before deserializing
                    if (string.IsNullOrWhiteSpace(orderRequest))
                    {
                        await Task.Delay(100, stoppingToken); // Small delay to prevent tight loop
                        continue;
                    }

                    //Deserialize 
                    OrderRequest? order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);
                    
                    if (order == null)
                    {
                        Console.WriteLine("Warning: Failed to deserialize order request");
                        continue;
                    }

                    //TODO:: Process Order
                    Console.WriteLine($"Info: OrderHandler => Processing the order for {order.productname}");
                    order.status = OrderStatus.COMPLETED;

                    //Write to ReadyToShip Queue
                    using (var producerWrapper = new ProducerWrapper(client, "readytoship"))
                    {
                        await producerWrapper.writeMessage(JsonConvert.SerializeObject(order));
                    }
                }
            }
        }
    }

}

