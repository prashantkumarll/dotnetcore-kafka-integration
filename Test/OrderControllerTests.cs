using Xunit;
using Moq;
using Azure.Messaging.ServiceBus;
using Api.Controllers;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void OrderController_Constructor_ShouldAcceptServiceBusClient()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockServiceBusClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldHavePostAsyncMethod()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();
            var controller = new OrderController(mockServiceBusClient.Object);

            // Act
            var methodInfo = controller.GetType().GetMethod("PostAsync");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldBeApiController()
        {
            // Arrange & Act
            var controllerType = typeof(OrderController);

            // Assert
            controllerType.Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public void OrderController_Constructor_WithNullServiceBusClient_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<System.ArgumentNullException>(() => new OrderController(null));
            exception.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var controllerType = typeof(OrderController);

            // Assert
            controllerType.Namespace.Should().Be("Api.Controllers");
        }
    }
}