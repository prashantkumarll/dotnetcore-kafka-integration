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
        public async Task CreateOrder_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = "ORD-123",
                CustomerName = "John Doe",
                Product = "Laptop",
                Quantity = 1,
                Price = 999.99m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().NotBeNull();
            
            _mockProcessOrdersService.Verify(x => x.ProcessOrderAsync(orderRequest), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_NullRequest_ReturnsBadRequest()
        {
            // Arrange
            OrderRequest? orderRequest = null;

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult?.Value.Should().Be("Order request cannot be null");
            
            _mockProcessOrdersService.Verify(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrder_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = "ORD-456",
                CustomerName = "Jane Smith",
                Product = "Phone",
                Quantity = 2,
                Price = 599.99m
            };

            _mockProcessOrdersService
                .Setup(x => x.ProcessOrderAsync(It.IsAny<OrderRequest>()))
                .ThrowsAsync(new System.Exception("Service Bus connection failed"));

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(500);
            objectResult?.Value.Should().Be("An error occurred while processing the order");
        }

        [Theory]
        [InlineData("", "John Doe", "Laptop", 1, 100)]
        [InlineData("ORD-123", "", "Laptop", 1, 100)]
        [InlineData("ORD-123", "John Doe", "", 1, 100)]
        [InlineData("ORD-123", "John Doe", "Laptop", 0, 100)]
        [InlineData("ORD-123", "John Doe", "Laptop", -1, 100)]
        public async Task CreateOrder_InvalidRequest_ReturnsBadRequest(string id, string customerName, string product, int quantity, decimal price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = id,
                CustomerName = customerName,
                Product = product,
                Quantity = quantity,
                Price = price
            };

            // Act
            var result = await _controller.CreateOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetOrderStatus_ValidId_ReturnsOkResult()
        {
            // Arrange
            var orderId = "ORD-123";
            var expectedStatus = "Processing";

            _mockProcessOrdersService
                .Setup(x => x.GetOrderStatusAsync(orderId))
                .ReturnsAsync(expectedStatus);

            // Act
            var result = await _controller.GetOrderStatus(orderId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().Be(expectedStatus);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task GetOrderStatus_InvalidId_ReturnsBadRequest(string orderId)
        {
            // Act
            var result = await _controller.GetOrderStatus(orderId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult?.Value.Should().Be("Order ID cannot be empty");
        }
    }
}