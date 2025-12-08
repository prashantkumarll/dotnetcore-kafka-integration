using Api.Controllers;
using Api.Models;
using Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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
        public async Task CreateOrder_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                CustomerId = "CUST001",
                ProductName = "Test Product",
                Quantity = 5,
                Price = 99.99m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeEquivalentTo(new { message = "Order processed successfully", orderId = orderRequest.OrderId });
        }

        [Fact]
        public async Task CreateOrder_WithNullRequest_ReturnsBadRequest()
        {
            // Arrange
            OrderRequest? nullRequest = null;

            // Act
            var result = await _controller.CreateOrder(nullRequest!);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult?.Value.Should().BeEquivalentTo(new { error = "Order request cannot be null" });
        }

        [Fact]
        public async Task CreateOrder_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                CustomerId = "CUST001",
                ProductName = "Test Product",
                Quantity = 1,
                Price = 50.00m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ThrowsAsync(new Exception("Service bus connection failed"));

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(500);
            objectResult?.Value.Should().BeEquivalentTo(new { error = "Failed to process order" });
        }

        [Theory]
        [InlineData("", "CUST001", "Product", 1, 10.00)]
        [InlineData("ORD001", "", "Product", 1, 10.00)]
        [InlineData("ORD001", "CUST001", "", 1, 10.00)]
        [InlineData("ORD001", "CUST001", "Product", 0, 10.00)]
        [InlineData("ORD001", "CUST001", "Product", -1, 10.00)]
        [InlineData("ORD001", "CUST001", "Product", 1, -10.00)]
        public async Task CreateOrder_WithInvalidData_ReturnsBadRequest(
            string orderId, string customerId, string productName, int quantity, decimal price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = orderId,
                CustomerId = customerId,
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateOrder_CallsProcessOrdersService()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                CustomerId = "CUST001",
                ProductName = "Test Product",
                Quantity = 2,
                Price = 25.50m
            };

            // Act
            await _controller.CreateOrder(orderRequest);

            // Assert
            _mockProcessOrdersService.Verify(
                x => x.ProcessOrderAsync(It.Is<OrderRequest>(o => 
                    o.OrderId == orderRequest.OrderId && 
                    o.CustomerId == orderRequest.CustomerId)),
                Times.Once);
        }
    }
}