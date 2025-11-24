using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Controllers;
using Api.Models;

namespace Api.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<ProducerConfig> _mockProducerConfig;
        private readonly Mock<ProducerWrapper> _mockProducerWrapper;

        public OrderControllerTests()
        {
            _mockProducerConfig = new Mock<ProducerConfig>();
            _mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "orderrequests");
        }

        [Fact]
        public async Task PostAsync_ValidOrderRequest_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>(); 
            _mockProducerWrapper.Verify(p => p.writeMessage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequestResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var controller = new OrderController(_mockProducerConfig.Object);
            controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void JsonSerialization_OrderRequest_CanSerializeAndDeserialize()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);
            var deserializedOrder = JsonConvert.DeserializeObject<OrderRequest>(serializedOrder);

            // Assert
            deserializedOrder.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ProducerThrowsException_HandlesError()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).ThrowsAsync(new Exception("Producer error"));

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => controller.PostAsync(orderRequest));
        }

        [Fact]
        public void OrderController_Constructor_InitializesWithProducerConfig()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

            // Assert
            controller.Should().NotBeNull();
        }
    }
}