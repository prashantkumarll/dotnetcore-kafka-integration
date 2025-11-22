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

namespace Api.Tests.Controllers
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
        public async Task PostAsync_ValidOrder_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "123",
                ProductId = "PROD001",
                Quantity = 5
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
        public async Task PostAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "123",
                ProductId = "PROD001",
                Quantity = 5
            };

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(p => p.writeMessage(It.Is<string>(s => 
                JsonConvert.DeserializeObject<OrderRequest>(s).CustomerId == "123")), Times.Once);
        }

        [Fact]
        public void Constructor_InitializesProducerConfig()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_NullOrder_ThrowsArgumentException()
        {
            // Arrange
            var controller = new OrderController(_mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.PostAsync(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task PostAsync_InvalidQuantity_ReturnsBadRequest(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "123",
                ProductId = "PROD001",
                Quantity = quantity
            };

            var controller = new OrderController(_mockProducerConfig.Object);
            controller.ModelState.AddModelError("Quantity", "Invalid quantity");

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}