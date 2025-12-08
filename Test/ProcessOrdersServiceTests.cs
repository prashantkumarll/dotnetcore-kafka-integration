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
        private readonly Mock<IProducerWrapper> _mockProducer;
        private readonly ProcessOrdersService _service;

        public ProcessOrdersServiceTests()
        {
            _mockProducer = new Mock<IProducerWrapper>();
            _service = new ProcessOrdersService(_mockProducer.Object);
        }

        [Fact]
        public async Task ProcessOrderAsync_ValidOrder_CallsProducerSendMessage()
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 2,
                Price = 50.00m
            };

            _mockProducer
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(order);

            // Assert
            _mockProducer.Verify(x => x.SendMessageAsync("orders", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ProcessOrderAsync_ValidOrder_SerializesOrderCorrectly()
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 3,
                Price = 75.50m
            };

            string capturedMessage = string.Empty;
            _mockProducer
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((topic, message) => capturedMessage = message)
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(order);

            // Assert
            capturedMessage.Should().NotBeEmpty();
            capturedMessage.Should().Contain("CUST001");
            capturedMessage.Should().Contain("PROD001");
            capturedMessage.Should().Contain("3");
            capturedMessage.Should().Contain("75.5");
        }

        [Fact]
        public async Task ProcessOrderAsync_ProducerThrowsException_PropagatesException()
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 1,
                Price = 25.00m
            };

            _mockProducer
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Producer error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.ProcessOrderAsync(order));
        }

        [Fact]
        public async Task ProcessOrderAsync_NullOrder_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ProcessOrderAsync(null));
        }
    }
}