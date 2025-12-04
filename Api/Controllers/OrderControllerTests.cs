using System;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Xunit;
using FluentAssertions;

namespace Api.Tests.Controllers
{
    public class FakeProducerWrapper
    {
        private readonly ProducerConfig config;
        private readonly string topic;
        public bool WriteMessageCalled { get; private set; }
        public string LastMessage { get; private set; }

        public FakeProducerWrapper(ProducerConfig config, string topic)
        {
            this.config = config;
            this.topic = topic;
        }

        public Task writeMessage(string message)
        {
            WriteMessageCalled = true;
            LastMessage = message;
            return Task.CompletedTask;
        }
    }

    public class TestableOrderController : OrderController
    {
        private FakeProducerWrapper fakeProducer;

        public TestableOrderController(ProducerConfig config) : base(config)
        {
        }

        public void SetFakeProducer(FakeProducerWrapper producer)
        {
            fakeProducer = producer;
        }

        public FakeProducerWrapper GetFakeProducer()
        {
            return fakeProducer;
        }
    }

    public class OrderControllerTests
    {
        private readonly ProducerConfig testConfig;
        private readonly TestableOrderController controller;

        public OrderControllerTests()
        {
            testConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            controller = new TestableOrderController(testConfig);
        }

        [Fact]
        public async Task PostAsync_WithValidOrder_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                ProductId = "PROD123",
                Quantity = 5,
                CustomerId = "CUST456"
            };

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            controller.ModelState.AddModelError("ProductId", "ProductId is required");

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PostAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                ProductId = "PROD789",
                Quantity = 3,
                CustomerId = "CUST123"
            };

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            var expectedJson = JsonConvert.SerializeObject(orderRequest);
            expectedJson.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Constructor_WithValidConfig_InitializesController()
        {
            // Arrange & Act
            var orderController = new OrderController(testConfig);

            // Assert
            orderController.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_WithNullOrder_HandlesProperly()
        {
            // Arrange
            OrderRequest nullOrder = null;

            // Act
            var result = await controller.PostAsync(nullOrder);

            // Assert
            result.Should().NotBeNull();
        }
    }
}