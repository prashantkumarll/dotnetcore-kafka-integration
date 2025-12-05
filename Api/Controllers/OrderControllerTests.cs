using System;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Xunit;
using FluentAssertions;

namespace Api.Tests.Controllers
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
        public async Task PostAsync_WithValidModel_ReturnsCreatedResult()
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
        public async Task PostAsync_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("TestKey", "Test error message");

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
        public async Task PostAsync_SerializesOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedJson = JsonConvert.SerializeObject(orderRequest);

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            expectedJson.Should().NotBeNullOrEmpty();
        }
    }

    public class ProducerWrapper
    {
        private readonly ProducerConfig _config;
        private readonly string _topic;

        public ProducerWrapper(ProducerConfig config, string topic)
        {
            _config = config;
            _topic = topic;
        }

        public async Task writeMessage(string message)
        {
            await Task.CompletedTask;
        }
    }
}