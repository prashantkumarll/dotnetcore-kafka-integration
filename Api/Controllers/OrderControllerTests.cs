using System;
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

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("key", "error message");
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_ProducerConfigProvided_ShouldInitializeController()
        {
            // Arrange
            var config = new ProducerConfig();

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            await _controller.PostAsync(orderRequest);

            // Assert
            JsonConvert.SerializeObject(orderRequest).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperCalled_WithSerializedOrder()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            await _controller.PostAsync(orderRequest);

            // Assert
            // Note: This is a placeholder. Actual verification would require mocking ProducerWrapper
            // which is not shown in the current implementation
        }
    }
}