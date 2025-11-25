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

namespace Api.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<ProducerConfig> _mockProducerConfig;
        private readonly Mock<ProducerWrapper> _mockProducerWrapper;

        public OrderControllerTests()
        {
            _mockProducerConfig = new Mock<ProducerConfig>();
            _mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "orderrequests");
        }

        [Fact]
        public async Task PostAsync_ValidOrderRequest_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                // Populate with valid test data based on actual OrderRequest properties
            };

            _mockProducerWrapper
                .Setup(p => p.writeMessage(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            var result = await controller.PostAsync(orderRequest);

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
            var orderRequest = new OrderRequest
            {
                // Populate with invalid test data
            };

            var controller = new OrderController(_mockProducerConfig.Object);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PostAsync_NullOrderRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var controller = new OrderController(_mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.PostAsync(null));
        }

        [Fact]
        public void Constructor_ValidProducerConfig_InitializesController()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ProducerWriteFailure_HandlesException()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                // Populate with valid test data
            };

            _mockProducerWrapper
                .Setup(p => p.writeMessage(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Producer write failed"));

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => controller.PostAsync(orderRequest));
        }
    }
}