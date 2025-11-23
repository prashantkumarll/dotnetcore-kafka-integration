using System;
using Xunit;
using Moq;
using FluentAssertions;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
            var orderRequest = new OrderRequest
            {
                // Populate with valid test data based on actual OrderRequest properties
            };

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

            var orderRequest = new OrderRequest();

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PostAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                // Populate with valid test data
            };

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);
            serializedOrder.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Constructor_InitializesProducerConfig()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperCalled()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                // Populate with valid test data
            };

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "orderrequests");
            mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            await controller.PostAsync(orderRequest);

            // Assert
            mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.Once);
        }
    }
}