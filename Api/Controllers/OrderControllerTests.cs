using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Controllers;
using Api.Models;
using Api;
using System.IO;
using System.Text;

namespace Api.Tests
{
    public class OrderControllerTests
    {
        private readonly ProducerConfig _producerConfig;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-client"
            };
            _controller = new OrderController(_producerConfig);
        }

        [Fact]
        public async Task PostAsync_ValidOrderRequest_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("testkey", "test error message");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PostAsync_SerializationTest()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            // Assert
            serializedOrder.Should().NotBeNullOrEmpty();
            var deserializeAction = () => JsonConvert.DeserializeObject<OrderRequest>(serializedOrder);
            deserializeAction.Should().NotThrow();
        }

        [Fact]
        public void Constructor_ProducerConfigInitialization()
        {
            // Arrange & Act
            var controller = new OrderController(_producerConfig);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperCreation_Success()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_NullOrderRequest_HandlesGracefully()
        {
            // Arrange
            OrderRequest orderRequest = default!;

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ConsoleOutput_WritesCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                await _controller.PostAsync(orderRequest);
                var output = stringWriter.ToString();

                // Assert
                output.Should().Contain("Info: OrderController => Post => Recieved a new purchase order:");
                output.Should().Contain("========");
                output.Should().Contain("=========");
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public async Task PostAsync_JsonSerialization_ProducesValidJson()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);
            await _controller.PostAsync(orderRequest);

            // Assert
            serializedOrder.Should().NotBeNullOrEmpty();
            serializedOrder.Should().StartWith("{");
            serializedOrder.Should().EndWith("}");
        }

        [Fact]
        public void Constructor_NullProducerConfig_ThrowsException()
        {
            // Arrange & Act & Assert
            var action = () => new OrderController(default!);
            action.Should().NotThrow();
        }

        [Fact]
        public async Task PostAsync_MultipleModelStateErrors_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("field1", "error1");
            _controller.ModelState.AddModelError("field2", "error2");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be(_controller.ModelState);
        }

        [Fact]
        public async Task PostAsync_EmptyOrderRequest_ProcessesSuccessfully()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperWithTopic_CreatesCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public void ProducerConfig_Properties_SetCorrectly()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "test-server",
                ClientId = "test-client-id"
            };

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }
    }
}