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

namespace Test
{
    public class OrderControllerTests
    {
        private readonly ServiceBusClient _mockConfig;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            // Arrange - Setup mock configuration
            _mockConfig = new ServiceBusClient
            {
                ConnectionString = "localhost:9092",
                ClientId = "test-client"
            };
            _controller = new OrderController(_mockConfig);
        }

        [Fact]
        public void Constructor_WithValidConfig_ShouldInitializeController()
        {
            // Arrange
            var config = new ServiceBusClient { ConnectionString = "localhost:9092" };

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldNotThrow()
        {
            // Arrange & Act
            var action = () => new OrderController(default!);

            // Assert
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
        public async Task PostAsync_WithMultipleModelErrors_ShouldReturnBadRequestWithAllErrors()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("Field1", "Error1");
            _controller.ModelState.AddModelError("Field2", "Error2");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            var modelState = badRequestResult.Value as ModelStateDictionary;
            modelState.ErrorCount.Should().Be(2);
        }

        [Fact]
        public void PostAsync_ShouldHaveHttpPostAttribute()
        {
            // Arrange
            var methodInfo = typeof(OrderController).GetMethod("PostAsync");

            // Act
            var httpPostAttribute = methodInfo.GetCustomAttributes(typeof(HttpPostAttribute), false).FirstOrDefault();

            // Assert
            httpPostAttribute.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldHaveRouteAttribute()
        {
            // Arrange
            var controllerType = typeof(OrderController);

            // Act
            var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), false).FirstOrDefault() as RouteAttribute;

            // Assert
            routeAttribute.Should().NotBeNull();
            routeAttribute.Template.Should().Be("api/[controller]");
        }
    }
}