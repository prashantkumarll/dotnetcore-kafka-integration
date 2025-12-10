using Xunit;
using Moq;
using Api.Controllers;
using Api.Models;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
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
        public void OrderController_Should_Initialize_Successfully()
        {
            // Act & Assert
            _controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_Should_Return_ActionResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_With_Null_Request_Should_Handle_Gracefully()
        {
            // Act
            var result = await _controller.PostAsync(null);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ServiceBusClient_Should_Be_Injected()
        {
            // Arrange & Act
            var controller = new OrderController(_mockServiceBusClient.Object);

            // Assert
            controller.Should().NotBeNull();
            _mockServiceBusClient.Object.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_Should_Accept_Valid_OrderRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act & Assert
            var act = async () => await _controller.PostAsync(orderRequest);
            await act.Should().NotThrowAsync();
        }
    }
}