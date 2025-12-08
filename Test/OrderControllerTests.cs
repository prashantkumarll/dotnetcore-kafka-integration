using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
        public async Task PlaceOrder_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                ProductName = "Test Product",
                Quantity = 2,
                Price = 99.99m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PlaceOrder(orderRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task PlaceOrder_WithNullRequest_ReturnsBadRequest()
        {
            // Arrange
            OrderRequest? orderRequest = null;

            // Act
            var result = await _controller.PlaceOrder(orderRequest!);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PlaceOrder_WithInvalidOrderId_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "",
                ProductName = "Test Product",
                Quantity = 2,
                Price = 99.99m
            };

            // Act
            var result = await _controller.PlaceOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PlaceOrder_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                ProductName = "Test Product",
                Quantity = 2,
                Price = 99.99m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ThrowsAsync(new System.Exception("Service unavailable"));

            // Act
            var result = await _controller.PlaceOrder(orderRequest);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(500);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task PlaceOrder_WithInvalidQuantity_ReturnsBadRequest(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "12345",
                ProductName = "Test Product",
                Quantity = quantity,
                Price = 99.99m
            };

            // Act
            var result = await _controller.PlaceOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}