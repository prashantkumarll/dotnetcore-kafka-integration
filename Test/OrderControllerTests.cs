using Xunit;
using Moq;
using FluentAssertions;
using Api.Controllers;
using Api.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Tests
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
        public void OrderController_ShouldBeInstantiated_WithServiceBusClient()
        {
            // Act & Assert
            _controller.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldInheritFromControllerBase()
        {
            // Act & Assert
            _controller.Should().BeAssignableTo<ControllerBase>();
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
        }

        [Fact]
        public async Task PostAsync_ShouldHandleNullOrderRequest()
        {
            // Act
            var result = await _controller.PostAsync(null);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldHaveServiceBusClientDependency()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(mockClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }
    }
}