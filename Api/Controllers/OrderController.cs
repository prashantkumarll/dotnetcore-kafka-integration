using System;
using System.Threading.Tasks;
using Api.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _queueName = "orderrequests";

        public OrderController(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] OrderRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Serialize 
            string serializedOrder = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Received a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            // Create a sender
            await using ServiceBusSender sender = _serviceBusClient.CreateSender(_queueName);

            // Create the message
            ServiceBusMessage message = new ServiceBusMessage(serializedOrder);

            // Send the message
            await sender.SendMessageAsync(message);

            return Created("TransactionId", "Your order is in progress");
        }
    }
}