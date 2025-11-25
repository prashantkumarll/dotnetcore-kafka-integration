using System;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

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
        public async Task PostAsync_NullOrderRequest_ThrowsArgumentException()
        {
            // Arrange
            var controller = new OrderController(_mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => controller.PostAsync(null));
        }

        [Fact]
        public async Task PostAsync_EmptyOrderRequest_ReturnsBadRequest()
        {
            // Arrange
            var emptyOrderRequest = new OrderRequest();
            var controller = new OrderController(_mockProducerConfig.Object);

            // Act
            var result = await controller.PostAsync(emptyOrderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}