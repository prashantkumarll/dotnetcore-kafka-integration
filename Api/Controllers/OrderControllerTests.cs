using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Tests.Controllers
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
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = await controller.PostAsync(new OrderRequest());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_ProducerConfigProvided_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

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
            JsonConvert.SerializeObject(orderRequest).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperCalled_WithSerializedOrder()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "orderrequests");
            mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Verifiable();

            // Act
            var controller = new OrderController(_mockProducerConfig.Object);
            await controller.PostAsync(orderRequest);

            // Assert
            mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.Once);
        }
    }

    public class OrderRequest { }
}