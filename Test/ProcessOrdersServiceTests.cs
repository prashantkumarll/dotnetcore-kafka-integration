using Xunit;
using Moq;
using FluentAssertions;
using Api.Services;
using Api.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ILogger<ProcessOrdersService>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly ProcessOrdersService _service;

        public ProcessOrdersServiceTests()
        {
            _mockLogger = new Mock<ILogger<ProcessOrdersService>>();
            _mockConfiguration = new Mock<IConfiguration>();
            
            _mockConfiguration
                .Setup(x => x["ServiceBus:ConnectionString"])
                .Returns("Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=test;SharedAccessKey=test");
            _mockConfiguration
                .Setup(x => x["ServiceBus:QueueName"])
                .Returns("orders");

            _service = new ProcessOrdersService(_mockLogger.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task ProcessOrderAsync_ValidOrder_CompletesSuccessfully()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = "ORD-001",
                CustomerName = "Test Customer",
                Product = "Test Product",
                Quantity = 1,
                Price = 100.00m
            };

            // Act & Assert - Should not throw
            var exception = await Record.ExceptionAsync(() => _service.ProcessOrderAsync(orderRequest));
            exception.Should().BeNull();
        }

        [Fact]
        public async Task ProcessOrderAsync_NullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            OrderRequest? orderRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ProcessOrderAsync(orderRequest));
        }

        [Fact]
        public async Task GetOrderStatusAsync_ValidId_ReturnsStatus()
        {
            // Arrange
            var orderId = "ORD-123";

            // Act
            var result = await _service.GetOrderStatusAsync(orderId);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeOneOf("Pending", "Processing", "Completed", "Failed");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task GetOrderStatusAsync_InvalidId_ThrowsArgumentException(string orderId)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetOrderStatusAsync(orderId));
        }

        [Fact]
        public void Constructor_MissingConfiguration_ThrowsException()
        {
            // Arrange
            var mockConfigWithMissingValues = new Mock<IConfiguration>();
            mockConfigWithMissingValues
                .Setup(x => x["ServiceBus:ConnectionString"])
                .Returns((string?)null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new ProcessOrdersService(_mockLogger.Object, mockConfigWithMissingValues.Object));
        }

        [Fact]
        public async Task ProcessOrderAsync_LargeOrder_HandledCorrectly()
        {
            // Arrange
            var largeOrder = new OrderRequest
            {
                Id = "ORD-LARGE-001",
                CustomerName = "Large Order Customer",
                Product = "Expensive Product",
                Quantity = 1000,
                Price = 99999.99m
            };

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _service.ProcessOrderAsync(largeOrder));
            exception.Should().BeNull();
        }

        [Fact]
        public async Task ProcessOrderAsync_MultipleOrdersSequentially_AllProcessed()
        {
            // Arrange
            var orders = new List<OrderRequest>
            {
                new OrderRequest { Id = "ORD-1", CustomerName = "Customer 1", Product = "Product 1", Quantity = 1, Price = 10.00m },
                new OrderRequest { Id = "ORD-2", CustomerName = "Customer 2", Product = "Product 2", Quantity = 2, Price = 20.00m },
                new OrderRequest { Id = "ORD-3", CustomerName = "Customer 3", Product = "Product 3", Quantity = 3, Price = 30.00m }
            };

            // Act & Assert
            foreach (var order in orders)
            {
                var exception = await Record.ExceptionAsync(() => _service.ProcessOrderAsync(order));
                exception.Should().BeNull();
            }
        }
    }
}