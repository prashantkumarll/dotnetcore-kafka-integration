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
        public void OrderController_Constructor_ShouldNotThrow()
        {
            // Arrange & Act
            var action = () => new OrderController(_mockServiceBusClient.Object);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void OrderController_ShouldHavePostAsyncMethod()
        {
            // Arrange
            var methodInfo = typeof(OrderController).GetMethod("PostAsync");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldInheritFromControllerBase()
        {
            // Assert
            _controller.Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public void OrderController_Constructor_WithNullServiceBusClient_ShouldNotThrow()
        {
            // Arrange & Act
            var action = () => new OrderController(null);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void OrderController_Instance_ShouldNotBeNull()
        {
            // Assert
            _controller.Should().NotBeNull();
        }
    }
}