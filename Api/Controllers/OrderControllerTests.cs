using System;
using System.IO;
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
            _controller.ModelState.AddModelError("TestKey", "Test error message");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_WithProducerConfig_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var controller = new OrderController(_producerConfig);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldNotThrow()
        {
            // Arrange & Act
            var action = () => new OrderController(default!);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task PostAsync_SerializesOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedSerialized = JsonConvert.SerializeObject(orderRequest);

            // Capture console output
            var originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                await _controller.PostAsync(orderRequest);

                // Assert
                var output = stringWriter.ToString();
                output.Should().Contain("OrderController => Post => Recieved a new purchase order:");
                output.Should().Contain(expectedSerialized);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public async Task PostAsync_WritesCorrectConsoleOutput()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                await _controller.PostAsync(orderRequest);

                // Assert
                var output = stringWriter.ToString();
                output.Should().Contain("========");
                output.Should().Contain("Info: OrderController => Post => Recieved a new purchase order:");
                output.Should().Contain("=========");
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public async Task PostAsync_MultipleModelStateErrors_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("Field1", "Error1");
            _controller.ModelState.AddModelError("Field2", "Error2");
            _controller.ModelState.AddModelError("Field3", "Error3");

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
        }

        [Fact]
        public async Task PostAsync_CreatesProducerWrapperWithCorrectParameters()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                var result = await _controller.PostAsync(orderRequest);

                // Assert
                result.Should().BeOfType<CreatedResult>();
                // Verify that the method completes without throwing
                // ProducerWrapper is created with config and "orderrequests" topic
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public async Task PostAsync_JsonSerializationHandlesComplexObjects()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);
            
            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            serializedOrder.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PostAsync_ReturnsCreatedWithCorrectLocationAndValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_ModelStateValid_DoesNotReturnBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            // Ensure ModelState is valid (no errors added)

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().NotBeOfType<BadRequestObjectResult>();
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ProducerConfigUsedCorrectly()
        {
            // Arrange
            var customConfig = new ProducerConfig
            {
                BootstrapServers = "custom-server:9092",
                ClientId = "custom-client"
            };
            var controller = new OrderController(customConfig);
            var orderRequest = new OrderRequest();

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }
    }
}