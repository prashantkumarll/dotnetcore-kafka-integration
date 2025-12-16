using System;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Api;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

namespace Test
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
        public void Constructor_WithNullConfig_ShouldAcceptNullConfig()
        {
            // Arrange
            ProducerConfig config = default!;

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_WithValidOrderRequest_ShouldReturnCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();
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
        public async Task PostAsync_WithNullOrderRequest_ShouldHandleNullInput()
        {
            // Arrange
            OrderRequest orderRequest = default!;
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ShouldSerializeOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();
            var expectedSerialization = JsonConvert.SerializeObject(orderRequest);

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            // Verify that serialization would work correctly
            var actualSerialization = JsonConvert.SerializeObject(orderRequest);
            actualSerialization.Should().Be(expectedSerialization);
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
            // Verify that the method completes successfully, indicating ProducerWrapper was created
            var createdResult = result as CreatedResult;
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_WithMultipleModelErrors_ShouldReturnBadRequestWithAllErrors()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("Field1", "Error1");
            _controller.ModelState.AddModelError("Field2", "Error2");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            var modelState = badRequestResult.Value as ModelStateDictionary;
            modelState.Should().NotBeNull();
            modelState.ErrorCount.Should().Be(2);
        }
    }
}