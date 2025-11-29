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
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockProducerConfig = new Mock<ProducerConfig>();
            _controller = new OrderController(_mockProducerConfig.Object);
        }

        [Fact]
        public async Task PostAsync_ValidOrderRequest_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "orderrequests");
            mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostAsync(orderRequest);

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
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void SerializeOrder_ShouldConvertToJsonString()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            // Assert
            serializedOrder.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PostAsync_ProducerWriteMessage_ShouldBeCalledOnce()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "orderrequests");
            mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            await _controller.PostAsync(orderRequest);

            // Assert
            mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithProducerConfig()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

            // Assert
            controller.Should().NotBeNull();
        }
    }
}