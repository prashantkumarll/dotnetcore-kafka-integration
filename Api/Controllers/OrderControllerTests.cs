using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;

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
            var controller = new OrderController(_mockProducerConfig.Object);
            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>(); 
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var controller = new OrderController(_mockProducerConfig.Object);
            controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await controller.PostAsync(new OrderRequest());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_WithProducerConfig_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var config = new ProducerConfig();
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(p => p.writeMessage(It.Is<string>(s => 
                !string.IsNullOrEmpty(s) && 
                JsonConvert.DeserializeObject(s) != null)), Times.Once);
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperCalled_WithSerializedOrder()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            _mockProducerWrapper.Verify(p => p.writeMessage(It.IsAny<string>()), Times.Once);
        }
    }
}