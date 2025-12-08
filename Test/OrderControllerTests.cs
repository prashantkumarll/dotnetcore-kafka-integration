using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Api.Controllers;
using Api.Models;
using Api.Services;
using System.Threading.Tasks;

namespace Test
{
    public class OrderControllerTests
    {
        private readonly Mock<IProcessOrdersService> _mockProcessOrdersService;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockProcessOrdersService = new Mock<IProcessOrdersService>();
            _controller = new OrderController(_mockProcessOrdersService.Object);
        }

        [Fact]
        public async Task ProcessOrder_WithValidOrder_ReturnsOkResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                CustomerId = "CUST-001",
                Amount = 100.50m,
                ProductName = "Test Product"
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().NotBeNull();
            
            _mockProcessOrdersService.Verify(
                x => x.ProcessOrderAsync(It.Is<OrderRequest>(o => o.OrderId == orderRequest.OrderId)),
                Times.Once);
        }

        [Fact]
        public async Task ProcessOrder_WithNullOrder_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.ProcessOrder(null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _mockProcessOrdersService.Verify(
                x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()),
                Times.Never);
        }

        [Fact]
        public async Task ProcessOrder_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-002",
                CustomerId = "CUST-002",
                Amount = 50.25m,
                ProductName = "Another Product"
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ThrowsAsync(new System.Exception("Service error"));

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(500);
        }

        [Theory]
        [InlineData("", "CUST-001", 100.0, "Product")]
        [InlineData("ORD-001", "", 100.0, "Product")]
        [InlineData("ORD-001", "CUST-001", -10.0, "Product")]
        [InlineData("ORD-001", "CUST-001", 100.0, "")]
        public async Task ProcessOrder_WithInvalidOrderData_ReturnsBadRequest(
            string orderId, string customerId, decimal amount, string productName)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = orderId,
                CustomerId = customerId,
                Amount = amount,
                ProductName = productName
            };

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task ProcessOrder_WithLargeAmount_ReturnsOkResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-LARGE",
                CustomerId = "CUST-VIP",
                Amount = 999999.99m,
                ProductName = "Expensive Product"
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _mockProcessOrdersService.Verify(
                x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()),
                Times.Once);
        }
    }
}