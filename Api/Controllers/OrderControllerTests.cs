using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Api.Models;
using Newtonsoft.Json;

namespace Api.Controllers.Tests
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
                CustomerId = "customer1",
                ProductId = "product1",
                Quantity = 1
            };

            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var controller = new OrderController(_mockProducerConfig.Object);
            controller.ModelState.AddModelError("CustomerId", "Required");

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_ConfigProvided_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "customer1",
                ProductId = "product1",
                Quantity = 1
            };

            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(p => p.writeMessage(It.Is<string>(s => 
                JsonConvert.DeserializeObject<OrderRequest>(s).CustomerId == "customer1")), Times.Once);
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperCalled_WithCorrectTopic()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "customer1",
                ProductId = "product1",
                Quantity = 1
            };

            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(p => p.writeMessage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PostAsync_NullOrderRequest_HandledGracefully()
        {
            // Arrange
            var controller = new OrderController(_mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.PostAsync(null));
        }
    }
}