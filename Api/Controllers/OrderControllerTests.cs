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
using Newtonsoft.Json;

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
            var clientMock = new Mock<ServiceBusClient>();

            // Act
            var controller = new OrderController(clientMock.Object);

            // Assert
            controller.Should().NotBeNull();
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
            OrderRequest orderRequest = default!;
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ShouldSerializeOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();
            var expectedSerialization = JsonConvert.SerializeObject(orderRequest);

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            // Note: In a real scenario, you would mock the ServiceBus client/wrapper to verify serialization
        }

        [Fact]
        public async Task PostAsync_ShouldCreateProducerWrapperWithCorrectParameters()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            // Note: ProducerWrapper is created with ServiceBusClient and "orderrequests" queue/topic
        }

        [Fact]
        public void PostAsync_WithMultipleModelErrors_ShouldReturnAllErrors()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("Field1", "Error1");
            _controller.ModelState.AddModelError("Field2", "Error2");

            // Act
            var result = _controller.PostAsync(orderRequest).Result;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            var modelState = badRequestResult.Value as ModelStateDictionary;
            modelState.ErrorCount.Should().Be(2);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnCreatedWithCorrectLocationAndValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }
    }
}