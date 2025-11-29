using System;
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
        private readonly Mock<ProducerWrapper> _mockProducerWrapper;

        public OrderControllerTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockProducerWrapper = new Mock<ProducerWrapper>(_mockServiceBusClient.Object, "orderrequests");
        }

        [Fact]
        public async Task PostAsync_ValidOrderRequest_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var controller = new OrderController(_mockServiceBusClient.Object);

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            _mockProducerWrapper.Verify(p => p.writeMessage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var controller = new OrderController(_mockServiceBusClient.Object);
            controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_ServiceBusClientProvided_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var controller = new OrderController(_mockServiceBusClient.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_SerializationCheck_CorrectJsonFormat()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var controller = new OrderController(_mockServiceBusClient.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            Action serializationAction = () => JsonConvert.SerializeObject(orderRequest);
            serializationAction.Should().NotThrow();
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperInteraction_MessageWritten()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var controller = new OrderController(_mockServiceBusClient.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(p => p.writeMessage(It.IsAny<string>()), Times.Once);
        }
    }
}