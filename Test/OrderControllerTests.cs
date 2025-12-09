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
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _orderController = new OrderController(_mockServiceBusClient.Object);
        }

        [Fact]
        public void OrderController_ShouldBeCreated_WithServiceBusClient()
        {
            // Arrange & Act
            var controller = new OrderController(_mockServiceBusClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ShouldReturnResult_WhenCalledWithValidOrder()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _orderController.PostAsync(orderRequest);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ShouldHandleNullOrderRequest()
        {
            // Arrange
            OrderRequest nullOrder = null;

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _orderController.PostAsync(nullOrder));
            exception.Should().BeNull();
        }

        [Fact]
        public void OrderController_ShouldAcceptServiceBusClient()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ShouldBeAsynchronous()
        {
            // Arrange
            var order = new OrderRequest();

            // Act
            var task = _orderController.PostAsync(order);

            // Assert
            task.Should().NotBeNull();
            await task;
        }
    }
}