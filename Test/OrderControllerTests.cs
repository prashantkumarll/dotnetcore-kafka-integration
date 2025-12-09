using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Azure.Messaging.ServiceBus;
using Api.Controllers;
using Api.Models;

namespace Test
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

            // Act & Assert
            controller.GetType().GetMethod("PostAsync").Should().NotBeNull();
        }

        [Fact]
        public void OrderController_PostAsync_ShouldBePublicMethod()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();
            var controller = new OrderController(mockServiceBusClient.Object);

            // Act
            var method = controller.GetType().GetMethod("PostAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockServiceBusClient.Object);

            // Assert
            controller.GetType().Namespace.Should().Be("Api.Controllers");
        }

        [Fact]
        public void OrderController_ShouldHaveCorrectClassName()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockServiceBusClient.Object);

            // Assert
            controller.GetType().Name.Should().Be("OrderController");
        }
    }
}