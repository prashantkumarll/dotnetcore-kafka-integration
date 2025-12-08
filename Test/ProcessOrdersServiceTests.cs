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
        public async Task ProcessOrderAsync_WithValidOrder_CallsProducerSendMessage()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                ProductName = "Laptop",
                Quantity = 1,
                Price = 999.99m
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(
                x => x.SendMessageAsync(
                    It.IsAny<string>(),
                    It.Is<string>(message => message.Contains("ORD-001"))),
                Times.Once);
        }

        [Fact]
        public async Task ProcessOrderAsync_WithNullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            OrderRequest? orderRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _service.ProcessOrderAsync(orderRequest!));
        }

        [Fact]
        public async Task ProcessOrderAsync_ProducerThrowsException_PropagatesException()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-002",
                ProductName = "Mouse",
                Quantity = 2,
                Price = 25.50m
            };

            var expectedException = new InvalidOperationException("Producer failed");
            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.ProcessOrderAsync(orderRequest));
            
            exception.Message.Should().Be("Producer failed");
        }

        [Fact]
        public async Task ProcessOrderAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-003",
                ProductName = "Keyboard",
                Quantity = 1,
                Price = 75.00m
            };

            string? capturedMessage = null;
            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((topic, message) => capturedMessage = message)
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            capturedMessage.Should().NotBeNull();
            capturedMessage.Should().Contain("ORD-003");
            capturedMessage.Should().Contain("Keyboard");
            capturedMessage.Should().Contain("75.00");
        }

        [Fact]
        public async Task ProcessOrderAsync_CallsProducerWithCorrectTopic()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-004",
                ProductName = "Monitor",
                Quantity = 1,
                Price = 299.99m
            };

            string? capturedTopic = null;
            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((topic, message) => capturedTopic = topic)
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            capturedTopic.Should().NotBeNullOrEmpty();
            _mockProducerWrapper.Verify(
                x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }
    }
}