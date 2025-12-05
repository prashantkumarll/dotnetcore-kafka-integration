using System;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Api.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly ProducerConfig _config;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            _controller = new OrderController(_config);
        }

        [Fact]
        public async Task PostAsync_WithValidOrder_ReturnsCreatedResult()
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
        public async Task PostAsync_WithInvalidModelState_ReturnsBadRequest()
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
        public async Task PostAsync_WithNullOrder_ReturnsBadRequest()
        {
            // Arrange
            OrderRequest orderRequest = null;
            _controller.ModelState.AddModelError("order", "Order cannot be null");
            
            // Act
            var result = await _controller.PostAsync(orderRequest);
            
            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Constructor_WithValidConfig_SetsConfigCorrectly()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "test-server:9092"
            };
            
            // Act
            var controller = new OrderController(config);
            
            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ThrowsException()
        {
            // Arrange
            ProducerConfig config = null;
            
            // Act & Assert
            Action act = () => new OrderController(config);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}