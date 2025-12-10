using Xunit;
using Moq;
using Api.Controllers;
using Api.Models;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using System.Threading.Tasks;

namespace Test
{
    public class OrderControllerTests
    {
        [Fact]
        public void OrderController_Should_Be_Instantiable_With_ServiceBusClient()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockServiceBusClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_Constructor_Should_Accept_ServiceBusClient()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();

            // Act & Assert
            var action = () => new OrderController(mockServiceBusClient.Object);
            action.Should().NotThrow();
        }

        [Fact]
        public void OrderController_Should_Have_PostAsync_Method()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();
            var controller = new OrderController(mockServiceBusClient.Object);

            // Act
            var method = typeof(OrderController).GetMethod("PostAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_PostAsync_Should_Be_Public_Method()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();

            // Act
            var methodInfo = typeof(OrderController).GetMethod("PostAsync");

            // Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_Should_Be_In_Correct_Namespace()
        {
            // Arrange
            var mockServiceBusClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockServiceBusClient.Object);

            // Assert
            controller.GetType().Namespace.Should().Be("Api.Controllers");
        }
    }
}