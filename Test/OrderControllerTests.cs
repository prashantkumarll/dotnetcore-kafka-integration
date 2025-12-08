using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
        public async Task SubmitOrder_ValidOrder_ReturnsOk()
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
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SubmitOrder(orderRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().NotBeNull();
            
            _mockProcessOrdersService.Verify(x => x.ProcessOrderAsync(orderRequest), Times.Once);
        }

        [Fact]
        public async Task SubmitOrder_NullOrder_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.SubmitOrder(null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _mockProcessOrdersService.Verify(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()), Times.Never);
        }

        [Fact]
        public async Task SubmitOrder_ServiceThrowsException_ReturnsInternalServerError()
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
                .ThrowsAsync(new System.Exception("Service error"));

            // Act
            var result = await _controller.SubmitOrder(orderRequest);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(500);
        }

        [Theory]
        [InlineData("", "PROD001", 1, 10.00)]
        [InlineData("CUST001", "", 1, 10.00)]
        [InlineData("CUST001", "PROD001", 0, 10.00)]
        [InlineData("CUST001", "PROD001", -1, 10.00)]
        [InlineData("CUST001", "PROD001", 1, -10.00)]
        public async Task SubmitOrder_InvalidOrderData_ReturnsBadRequest(string customerId, string productId, int quantity, decimal price)
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
            var result = await _controller.SubmitOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _mockProcessOrdersService.Verify(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()), Times.Never);
        }
    }
}