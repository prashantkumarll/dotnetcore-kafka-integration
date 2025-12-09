using Xunit;
using Moq;
using FluentAssertions;
using Api.Controllers;
using Api.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public void OrderController_ShouldBeInstantiated_WithValidServiceBusClient()
        {
            // Arrange & Act
            var controller = new OrderController(_mockServiceBusClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ShouldReturnActionResult_WhenCalled()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult>();
        }

        [Fact]
        public async Task PostAsync_ShouldHandleNullOrderRequest()
        {
            // Arrange
            OrderRequest nullOrder = null;

            // Act
            var result = await _controller.PostAsync(nullOrder);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldHavePostAsyncMethod()
        {
            // Arrange & Act
            var method = typeof(OrderController).GetMethod("PostAsync");

            // Assert
            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(Task<ActionResult>));
        }

        [Fact]
        public void OrderController_Constructor_ShouldAcceptServiceBusClient()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockClient.Object);

            // Assert
            controller.Should().NotBeNull();
            controller.Should().BeOfType<OrderController>();
        }
    }
}