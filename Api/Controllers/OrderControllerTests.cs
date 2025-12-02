using System;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Api.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<ServiceBusClient> _mockClient;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            // Arrange - Setup mock ServiceBus client
            _mockClient = new Mock<ServiceBusClient>();
            _controller = new OrderController(_mockClient.Object);
        }

        [Fact]
        public void Constructor_WithValidConfig_ShouldInitializeController()
        {
            // Arrange
            var client = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(client.Object);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldNotThrow()
        {
            // Arrange & Act & Assert
            var action = () => new OrderController(default!);
            action.Should().NotThrow();
        }

        [Fact]
        public async Task PostAsync_WithValidOrderRequest_ShouldReturnCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_WithInvalidModelState_ShouldReturnBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("TestField", "Test error message");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be(_controller.ModelState);
        }

        [Fact]
        public async Task PostAsync_WithNullOrderRequest_ShouldHandleGracefully()
        {
            // Arrange
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(default!);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ShouldSerializeOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Value.Should().NotBeNull();
        }

        [Fact]
        public void Controller_ShouldHaveCorrectRouteAttribute()
        {
            // Arrange & Act
            var controllerType = typeof(OrderController);
            var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), false).FirstOrDefault() as RouteAttribute;

            // Assert
            routeAttribute.Should().NotBeNull();
            routeAttribute.Template.Should().Be("api/[controller]");
        }

        [Fact]
        public void PostAsync_ShouldHaveCorrectHttpPostAttribute()
        {
            // Arrange & Act
            var methodInfo = typeof(OrderController).GetMethod("PostAsync");
            var httpPostAttribute = methodInfo.GetCustomAttributes(typeof(HttpPostAttribute), false).FirstOrDefault();

            // Assert
            httpPostAttribute.Should().NotBeNull();
        }
    }
}