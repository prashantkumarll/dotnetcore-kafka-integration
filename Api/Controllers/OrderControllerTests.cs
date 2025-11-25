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
        public async Task PostAsync_ProducerThrowsException_HandlesError()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _mockProducerWrapper.Setup(p => p.writeMessage(It.IsAny<string>())).ThrowsAsync(new Exception("Kafka error"));

            var controller = new OrderController(_mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => controller.PostAsync(orderRequest));
        }

        [Fact]
        public async Task PostAsync_EmptyOrderRequest_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}