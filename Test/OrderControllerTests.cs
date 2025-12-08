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
        public async Task CreateOrder_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 5,
                Price = 99.99m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateOrder_WithNullRequest_ReturnsBadRequest()
        {
            // Arrange
            OrderRequest? orderRequest = null;

            // Act
            var result = await _controller.CreateOrder(orderRequest!);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateOrder_WhenServiceFails_ReturnsInternalServerError()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 1,
                Price = 10.00m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ThrowsAsync(new System.Exception("Service unavailable"));

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(500);
        }

        [Theory]
        [InlineData("", "PROD001", 1, 10.00)]
        [InlineData("CUST001", "", 1, 10.00)]
        [InlineData("CUST001", "PROD001", 0, 10.00)]
        [InlineData("CUST001", "PROD001", 1, -1.00)]
        public async Task CreateOrder_WithInvalidData_ReturnsBadRequest(
            string customerId, string productId, int quantity, decimal price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity,
                Price = price
            };

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateOrder_VerifiesServiceCall()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 2,
                Price = 50.00m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ReturnsAsync(true);

            // Act
            await _controller.CreateOrder(orderRequest);

            // Assert
            _mockProcessOrdersService.Verify(
                x => x.ProcessOrderAsync(It.Is<OrderRequest>(o => 
                    o.CustomerId == orderRequest.CustomerId &&
                    o.ProductId == orderRequest.ProductId &&
                    o.Quantity == orderRequest.Quantity &&
                    o.Price == orderRequest.Price)),
                Times.Once);
        }
    }
}