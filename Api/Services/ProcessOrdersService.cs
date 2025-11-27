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
        private readonly ServiceBusClient serviceBusClient;
        public ProcessOrdersService(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("OrderProcessing Service Started");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumerHelper = new ConsumerWrapper(serviceBusClient, "orderrequests");
                string orderRequest = await consumerHelper.readMessage();

                //Deserilaize 
                OrderRequest order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);

                //TODO:: Process Order
                Console.WriteLine($"Info: OrderHandler => Processing the order for {order.productname}");
                order.status = OrderStatus.COMPLETED;

                //Write to ReadyToShip Queue

                var producerWrapper = new ProducerWrapper(serviceBusClient, "readytoship");
                await producerWrapper.writeMessage(JsonConvert.SerializeObject(order));
            }
        }
    }

}

