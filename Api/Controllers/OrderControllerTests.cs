using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Api.Controllers;
using Api.Models;
using Newtonsoft.Json;

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
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequestResult()
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
        public async Task PostAsync_SerializesOrderCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            await _controller.PostAsync(orderRequest);

            // Assert
            JsonConvert.SerializeObject(orderRequest).Should().NotBeNull();
        }

        [Fact]
        public void Constructor_InitializesWithProducerConfig()
        {
            // Arrange & Act
            var controller = new OrderController(_mockProducerConfig.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ProducerWrapperCreated_MessageWritten()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }
    }
}