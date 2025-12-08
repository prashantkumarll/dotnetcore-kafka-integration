using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Api.Controllers;
using Api.Services;
using Api.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Test.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IProcessOrdersService> _mockProcessOrdersService;
        private readonly Mock<ILogger<OrderController>> _mockLogger;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockProcessOrdersService = new Mock<IProcessOrdersService>();
            _mockLogger = new Mock<ILogger<OrderController>>();
            _controller = new OrderController(_mockProcessOrdersService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessOrder_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 2,
                Price = 29.99m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(new { message = "Order processed successfully", orderId = orderRequest.OrderId });
        }

        [Fact]
        public async Task ProcessOrder_WithNullRequest_ReturnsBadRequest()
        {
            // Arrange
            OrderRequest? orderRequest = null;

            // Act
            var result = await _controller.ProcessOrder(orderRequest!);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().BeEquivalentTo(new { error = "Invalid order request" });
        }

        [Fact]
        public async Task ProcessOrder_WithInvalidOrderId_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "",
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 2,
                Price = 29.99m
            };

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task ProcessOrder_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 2,
                Price = 29.99m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ThrowsAsync(new System.Exception("Service unavailable"));

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task ProcessOrder_WithInvalidQuantity_ReturnsBadRequest(int invalidQuantity)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = invalidQuantity,
                Price = 29.99m
            };

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10.50)]
        public async Task ProcessOrder_WithInvalidPrice_ReturnsBadRequest(decimal invalidPrice)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 2,
                Price = invalidPrice
            };

            // Act
            var result = await _controller.ProcessOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}