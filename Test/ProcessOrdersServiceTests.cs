using Xunit;
using Moq;
using FluentAssertions;
using Api.Services;
using Api.Models;
using System.Threading.Tasks;

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
        public async Task ProcessOrderAsync_WithValidOrder_CallsProducerWrapper()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                CustomerId = "CUST-001",
                Amount = 100.50m,
                ProductName = "Test Product"
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(
                x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task ProcessOrderAsync_WithNullOrder_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _service.ProcessOrderAsync(null));
        }

        [Fact]
        public async Task ProcessOrderAsync_WhenProducerThrowsException_PropagatesException()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-002",
                CustomerId = "CUST-002",
                Amount = 75.25m,
                ProductName = "Another Product"
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new System.Exception("Producer error"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(
                () => _service.ProcessOrderAsync(orderRequest));
        }

        [Fact]
        public async Task ProcessOrderAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-003",
                CustomerId = "CUST-003",
                Amount = 200.75m,
                ProductName = "Serialization Test Product"
            };

            string capturedMessage = null;
            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((topic, message) => capturedMessage = message)
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            capturedMessage.Should().NotBeNullOrEmpty();
            capturedMessage.Should().Contain(orderRequest.OrderId);
            capturedMessage.Should().Contain(orderRequest.CustomerId);
            capturedMessage.Should().Contain(orderRequest.ProductName);
        }

        [Fact]
        public async Task ProcessOrderAsync_CallsProducerWithCorrectTopic()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-004",
                CustomerId = "CUST-004",
                Amount = 150.00m,
                ProductName = "Topic Test Product"
            };

            string capturedTopic = null;
            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((topic, message) => capturedTopic = topic)
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            capturedTopic.Should().NotBeNullOrEmpty();
            capturedTopic.Should().Be("orders");
        }
    }

    // Mock interface for IProducerWrapper
    public interface IProducerWrapper
    {
        Task SendMessageAsync(string topic, string message);
    }
}