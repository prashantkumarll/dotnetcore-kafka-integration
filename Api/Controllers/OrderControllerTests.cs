using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Api.Controllers;
using Api.Models;

namespace Api.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusSender> _mockServiceBusSender;

        public OrderControllerTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusSender = new Mock<ServiceBusSender>();
            _mockServiceBusClient
                .Setup(c => c.CreateSender("orderrequests"))
                .Returns(_mockServiceBusSender.Object);
        }

        [Fact]
        public async Task PostAsync_ValidOrderRequest_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                // Populate with valid test data based on actual OrderRequest properties
            };

            _mockServiceBusSender
                .Setup(s => s.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new OrderController(_mockServiceBusClient.Object);

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                // Populate with invalid test data
            };

            var controller = new OrderController(_mockServiceBusClient.Object);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PostAsync_NullOrderRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var controller = new OrderController(_mockServiceBusClient.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.PostAsync(null));
        }

        [Fact]
        public void Constructor_ValidProducerConfig_InitializesController()
        {
            // Arrange & Act
            var controller = new OrderController(_mockServiceBusClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ProducerWriteFailure_HandlesException()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                // Populate with valid test data
            };

            _mockServiceBusSender
                .Setup(s => s.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Producer write failed"));

            var controller = new OrderController(_mockServiceBusClient.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => controller.PostAsync(orderRequest));
        }
    }
}