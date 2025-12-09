using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Test
{
    public class OrderControllerTests
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _controller = new OrderController(_mockServiceBusClient.Object);
        }

        [Fact]
        public void OrderController_ShouldBeInitialized()
        {
            // Act & Assert
            _controller.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldHavePostAsyncMethod()
        {
            // Arrange
            var methodInfo = typeof(OrderController).GetMethod("PostAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.ReturnType.Should().Be(typeof(Task<IActionResult>));
        }

        [Fact]
        public async Task PostAsync_WithNullOrder_ShouldHandleGracefully()
        {
            // Arrange
            OrderRequest nullOrder = null;

            // Act
            var result = await _controller.PostAsync(nullOrder);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_WithValidOrder_ShouldReturnResult()
        {
            // Arrange
            var order = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(order);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IActionResult>();
        }

        [Fact]
        public void OrderController_ShouldAcceptServiceBusClient()
        {
            // Arrange & Act
            var controller = new OrderController(_mockServiceBusClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }
    }
}