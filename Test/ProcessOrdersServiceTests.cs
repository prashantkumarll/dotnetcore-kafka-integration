using Xunit;
using Moq;
using FluentAssertions;
using Api.Services;
using Api.Models;
using Api;
using System.Threading.Tasks;
using System;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<IProducerWrapper> _mockProducerWrapper;
        private readonly ProcessOrdersService _service;

        public ProcessOrdersServiceTests()
        {
            _mockProducerWrapper = new Mock<IProducerWrapper>();
            _service = new ProcessOrdersService(_mockProducerWrapper.Object);
        }

        [Fact]
        public async Task ProcessOrderAsync_WithValidOrder_ReturnsTrue()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 3,
                Price = 29.99m
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ProcessOrderAsync(orderRequest);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ProcessOrderAsync_WithNullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            OrderRequest? orderRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.ProcessOrderAsync(orderRequest!));
        }

        [Fact]
        public async Task ProcessOrderAsync_WhenProducerFails_ThrowsException()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 1,
                Price = 15.99m
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Producer error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _service.ProcessOrderAsync(orderRequest));
        }

        [Fact]
        public async Task ProcessOrderAsync_CallsProducerWithCorrectParameters()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST123",
                ProductId = "PROD456",
                Quantity = 2,
                Price = 75.50m
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(
                x => x.SendMessageAsync("orders", It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task ProcessOrderAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST789",
                ProductId = "PROD999",
                Quantity = 5,
                Price = 199.99m
            };

            string capturedMessage = string.Empty;
            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((topic, message) => capturedMessage = message)
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            capturedMessage.Should().NotBeNullOrEmpty();
            capturedMessage.Should().Contain("CUST789");
            capturedMessage.Should().Contain("PROD999");
            capturedMessage.Should().Contain("199.99");
        }
    }
}