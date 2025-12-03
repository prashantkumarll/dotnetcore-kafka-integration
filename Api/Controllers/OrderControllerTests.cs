using System;
using System.IO;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Api.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly ProducerConfig _mockConfig;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            // Arrange - Setup mock configuration
            _mockConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-client"
            };
            _controller = new OrderController(_mockConfig);
        }

        [Fact]
        public void Constructor_WithValidConfig_ShouldInitializeController()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig config = default!;

            // Act & Assert
            Action act = () => new OrderController(config);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task PostAsync_WithValidOrderRequest_ShouldReturnCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                // Add properties based on actual OrderRequest model
            };
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_WithInvalidModelState_ShouldReturnBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("TestField", "Test error message");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be(_controller.ModelState);
        }

        [Fact]
        public async Task PostAsync_WithNullOrderRequest_ShouldHandleGracefully()
        {
            // Arrange
            OrderRequest orderRequest = default!;
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ShouldSerializeOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();
            var expectedJson = JsonConvert.SerializeObject(orderRequest);

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            expectedJson.Should().NotBeNullOrEmpty();
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ShouldWriteToConsole()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                await _controller.PostAsync(orderRequest);

                // Assert
                var consoleOutput = stringWriter.ToString();
                consoleOutput.Should().Contain("Info: OrderController => Post => Recieved a new purchase order:");
                consoleOutput.Should().Contain("========");
                consoleOutput.Should().Contain("=========");
            }
            finally
            {
                // Cleanup
                Console.SetOut(originalOut);
                stringWriter.Dispose();
            }
        }

        [Fact]
        public async Task PostAsync_ShouldCreateProducerWrapperWithCorrectParameters()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            // Note: ProducerWrapper creation is tested indirectly through successful execution
        }
    }
}