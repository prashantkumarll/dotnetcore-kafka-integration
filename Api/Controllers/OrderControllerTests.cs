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
using Api;

namespace Api.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<ProducerConfig> _mockProducerConfig;
        private readonly Mock<ProducerWrapper> _mockProducerWrapper;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockProducerConfig = new Mock<ProducerConfig>();
            _mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "orderrequests");
            _controller = new OrderController(_mockProducerConfig.Object);
        }

        [Fact]
        public async Task PostAsync_ValidOrderRequest_SendsMessageToKafka()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var mockProducer = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "orderrequests");
            mockProducer.Setup(p => p.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            mockProducer.Verify(p => p.writeMessage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PostAsync_NullOrderRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.PostAsync(null));
        }

        [Fact]
        public void Constructor_ValidProducerConfig_InitializesWithoutErrors()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

            // Assert
            controller.Should().NotBeNull();
        }
    }
}