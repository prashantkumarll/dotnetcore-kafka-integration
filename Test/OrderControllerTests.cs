using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
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
        public void OrderController_ShouldInstantiateSuccessfully()
        {
            // Act & Assert
            _controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ShouldBePublicMethod()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var methodInfo = typeof(OrderController).GetMethod("PostAsync");

            // Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_ShouldHaveCorrectConstructor()
        {
            // Act
            var constructors = typeof(OrderController).GetConstructors();

            // Assert
            constructors.Should().HaveCount(1);
            var constructor = constructors[0];
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be(typeof(ServiceBusClient));
        }

        [Fact]
        public void OrderController_ShouldAcceptServiceBusClientInConstructor()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldBeInCorrectNamespace()
        {
            // Act
            var type = typeof(OrderController);

            // Assert
            type.Namespace.Should().Be("Api.Controllers");
        }
    }
}