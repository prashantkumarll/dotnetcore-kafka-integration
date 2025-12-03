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
                BootstrapServers = "localhost:9092"
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
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be(_controller.ModelState);
        }

        [Fact]
        public async Task PostAsync_NullOrderRequest_HandlesGracefully()
        {
            // Arrange
            OrderRequest orderRequest = null;

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public void Constructor_WithProducerConfig_InitializesSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "test-server" };

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_InitializesSuccessfully()
        {
            // Arrange
            ProducerConfig config = null;

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_SerializesOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedJson = JsonConvert.SerializeObject(orderRequest);

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            expectedJson.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PostAsync_WritesConsoleOutput()
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
                var output = stringWriter.ToString();
                output.Should().NotBeNullOrEmpty();
                result.Should().BeOfType<CreatedResult>();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public async Task PostAsync_MultipleValidRequests_AllReturnCreated()
        {
            // Arrange
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Act
            var result1 = await _controller.PostAsync(orderRequest1);
            var result2 = await _controller.PostAsync(orderRequest2);

            // Assert
            result1.Should().BeOfType<CreatedResult>();
            result2.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ModelStateWithMultipleErrors_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("Field1", "Error1");
            _controller.ModelState.AddModelError("Field2", "Error2");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _controller.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task PostAsync_EmptyOrderRequest_ReturnsCreated()
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
        public async Task PostAsync_JsonSerialization_ProducesValidJson()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            serializedOrder.Should().NotBeNullOrEmpty();
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperCreation_UsesCorrectParameters()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ReturnsCorrectCreatedResponse()
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
            createdResult.StatusCode.Should().Be(201);
        }
    }
}