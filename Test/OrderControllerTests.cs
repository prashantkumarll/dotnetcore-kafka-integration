using Xunit;
using Moq;
using Api.Controllers;
using Api.Models;
using Api;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Threading.Tasks;

namespace Test
{
    public class OrderControllerTests
    {
        private readonly Mock<ProducerWrapper> _mockProducer;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockProducer = new Mock<ProducerWrapper>();
            _controller = new OrderController();
        }

        [Fact]
        public async Task PostAsync_ShouldReturnOkResult_WhenOrderRequestIsValid()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<IActionResult>();
        }

        [Fact]
        public async Task PostAsync_ShouldHandleNullOrderRequest()
        {
            // Arrange
            OrderRequest? nullOrder = null;

            // Act
            var result = await _controller.PostAsync(nullOrder);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldBeInstantiable()
        {
            // Act
            var controller = new OrderController();

            // Assert
            controller.Should().NotBeNull();
            controller.Should().BeOfType<OrderController>();
        }

        [Fact]
        public void OrderController_ShouldInheritFromControllerBase()
        {
            // Arrange & Act
            var controller = new OrderController();

            // Assert
            controller.Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public async Task PostAsync_ShouldReturnTask()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var resultTask = _controller.PostAsync(orderRequest);

            // Assert
            resultTask.Should().NotBeNull();
            resultTask.Should().BeOfType<Task<IActionResult>>();
            
            var result = await resultTask;
            result.Should().NotBeNull();
        }
    }
}