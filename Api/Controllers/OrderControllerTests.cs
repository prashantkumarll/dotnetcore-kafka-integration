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
        public async Task PostAsync_InvalidModelState_ReturnsBadRequestResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("TestError", "Test error message");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_WithProducerConfig_ShouldInitializeController()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldAcceptNull()
        {
            // Arrange & Act
            var controller = new OrderController(default!);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ValidRequest_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                var result = await _controller.PostAsync(orderRequest);

                // Assert
                result.Should().BeOfType<CreatedResult>();
                var output = stringWriter.ToString();
                output.Should().Contain("Info: OrderController => Post => Recieved a new purchase order:");
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public async Task PostAsync_WithMultipleModelErrors_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("Field1", "Error 1");
            _controller.ModelState.AddModelError("Field2", "Error 2");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be(_controller.ModelState);
        }

        [Fact]
        public async Task PostAsync_LogsSerializedOrder_ContainsExpectedOutput()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedJson = JsonConvert.SerializeObject(orderRequest);
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                await _controller.PostAsync(orderRequest);

                // Assert
                var output = stringWriter.ToString();
                output.Should().Contain(expectedJson);
                output.Should().Contain("========");
                output.Should().Contain("=========");
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public async Task PostAsync_ClearsModelStateErrors_AfterValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("TestField", "Test error");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _controller.ModelState.Clear();
            
            // Act again with cleared state
            var secondResult = await _controller.PostAsync(orderRequest);
            
            // Assert
            secondResult.Should().BeOfType<CreatedResult>();
        }

        [Theory]
        [InlineData("localhost:9092")]
        [InlineData("test-server:9092")]
        [InlineData("kafka-cluster:9092")]
        public void Constructor_WithDifferentBootstrapServers_ShouldInitialize(string bootstrapServer)
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = bootstrapServer };

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_CreatedResult_HasCorrectLocationAndValue()
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
        public async Task PostAsync_ModelStateValid_ReturnsCreatedWithCorrectMessage()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_ConsoleOutput_ContainsExpectedStructure()
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

                // Assert
                var output = stringWriter.ToString();
                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                lines.Should().Contain(line => line.Contains("========"));
                lines.Should().Contain(line => line.Contains("Info: OrderController => Post => Recieved a new purchase order:"));
                lines.Should().Contain(line => line.Contains("========="));
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }
}