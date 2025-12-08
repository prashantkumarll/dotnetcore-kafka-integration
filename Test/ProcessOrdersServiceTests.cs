using Xunit;
using Moq;
using FluentAssertions;
using Api.Services;
using Api.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace Test.Services
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<IProducerWrapper> _mockProducerWrapper;
        private readonly Mock<ILogger<ProcessOrdersService>> _mockLogger;
        private readonly ProcessOrdersService _service;

        public ProcessOrdersServiceTests()
        {
            _mockProducerWrapper = new Mock<IProducerWrapper>();
            _mockLogger = new Mock<ILogger<ProcessOrdersService>>();
            _service = new ProcessOrdersService(_mockProducerWrapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessOrderAsync_WithValidOrder_ReturnsTrue()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                CustomerId = "CUST-001",
                ProductId = "PROD-001",
                Quantity = 3,
                Price = 99.99m
            };

            _mockProducerWrapper
                .Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ProcessOrderAsync(orderRequest);

            // Assert
            result.Should().BeTrue();
            _mockProducerWrapper.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ProcessOrderAsync_ProducerThrowsException_ReturnsFalse()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                CustomerId = "CUST-001",
                ProductId = "PROD-001",
                Quantity = 3,
                Price = 99.99m
            };

            _mockProducerWrapper
                .Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Message broker unavailable"));

            // Act
            var result = await _service.ProcessOrderAsync(orderRequest);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ProcessOrderAsync_WithNullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            OrderRequest? orderRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ProcessOrderAsync(orderRequest!));
        }

        [Fact]
        public async Task ProcessOrderAsync_VerifyMessageContent_CallsProducerWithCorrectParameters()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-123",
                CustomerId = "CUST-456",
                ProductId = "PROD-789",
                Quantity = 5,
                Price = 149.99m
            };

            string capturedTopic = "";
            string capturedMessage = "";

            _mockProducerWrapper
                .Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((topic, message) =>
                {
                    capturedTopic = topic;
                    capturedMessage = message;
                })
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            capturedTopic.Should().NotBeNullOrEmpty();
            capturedMessage.Should().Contain(orderRequest.OrderId);
            capturedMessage.Should().Contain(orderRequest.CustomerId);
            capturedMessage.Should().Contain(orderRequest.ProductId);
        }

        [Theory]
        [InlineData("", "CUST-001", "PROD-001", 1, 10.0)]
        [InlineData("ORD-001", "", "PROD-001", 1, 10.0)]
        [InlineData("ORD-001", "CUST-001", "", 1, 10.0)]
        public async Task ProcessOrderAsync_WithIncompleteData_StillProcesses(string orderId, string customerId, string productId, int quantity, decimal price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = orderId,
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity,
                Price = price
            };

            _mockProducerWrapper
                .Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ProcessOrderAsync(orderRequest);

            // Assert
            result.Should().BeTrue();
        }
    }
}