using Api.Models;
using Api.Services;
using FluentAssertions;
using Moq;
using Xunit;
using Api;

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
                OrderId = "ORD001",
                CustomerId = "CUST001",
                ProductName = "Laptop",
                Quantity = 1,
                Price = 1299.99m
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(
                x => x.SendMessageAsync(It.Is<string>(message => 
                    message.Contains(orderRequest.OrderId) && 
                    message.Contains(orderRequest.CustomerId))),
                Times.Once);
        }

        [Fact]
        public async Task ProcessOrderAsync_WithNullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            OrderRequest? nullOrder = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.ProcessOrderAsync(nullOrder!));
        }

        [Fact]
        public async Task ProcessOrderAsync_WhenProducerFails_PropagatesException()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD001",
                CustomerId = "CUST001",
                ProductName = "Product",
                Quantity = 1,
                Price = 10.00m
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Producer connection failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _service.ProcessOrderAsync(orderRequest));

            exception.Message.Should().Be("Producer connection failed");
        }

        [Fact]
        public async Task ProcessOrderAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD123",
                CustomerId = "CUST456",
                ProductName = "Test Product",
                Quantity = 3,
                Price = 45.67m
            };

            string? capturedMessage = null;
            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>()))
                .Callback<string>(message => capturedMessage = message)
                .Returns(Task.CompletedTask);

            // Act
            await _service.ProcessOrderAsync(orderRequest);

            // Assert
            capturedMessage.Should().NotBeNull();
            capturedMessage.Should().Contain("\"OrderId\":\"ORD123\"");
            capturedMessage.Should().Contain("\"CustomerId\":\"CUST456\"");
            capturedMessage.Should().Contain("\"ProductName\":\"Test Product\"");
            capturedMessage.Should().Contain("\"Quantity\":3");
            capturedMessage.Should().Contain("\"Price\":45.67");
        }

        [Theory]
        [InlineData(1, 10.00)]
        [InlineData(100, 999.99)]
        [InlineData(5, 0.01)]
        public async Task ProcessOrderAsync_WithDifferentQuantitiesAndPrices_ProcessesSuccessfully(
            int quantity, decimal price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD001",
                CustomerId = "CUST001",
                ProductName = "Variable Product",
                Quantity = quantity,
                Price = price
            };

            _mockProducerWrapper
                .Setup(x => x.SendMessageAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = () => _service.ProcessOrderAsync(orderRequest);

            // Assert
            await act.Should().NotThrowAsync();
            _mockProducerWrapper.Verify(x => x.SendMessageAsync(It.IsAny<string>()), Times.Once);
        }
    }
}