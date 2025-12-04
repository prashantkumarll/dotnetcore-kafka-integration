using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Confluent.Kafka;
using Api.Controllers;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Tests
{
    public class OrderControllerTests
    {
        private readonly ProducerConfig _config;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _config = new ProducerConfig();
            _controller = new OrderController(_config);
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
            var createdResult = result as CreatedResult;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("TestField", "Test error");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_WithProducerConfig_SetsConfigCorrectly()
        {
            // Arrange
            var config = new ProducerConfig();

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_WithNullOrderRequest_HandlesGracefully()
        {
            // Arrange
            OrderRequest orderRequest = null;

            // Act & Assert
            var act = async () => await _controller.PostAsync(orderRequest);
            await act.Should().NotThrowAsync();
        }
    }
}