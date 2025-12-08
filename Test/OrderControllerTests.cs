using Api.Controllers;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class OrderControllerTests
    {
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _controller = new OrderController();
        }

        [Fact]
        public void OrderController_ShouldBeInstantiable()
        {
            // Act & Assert
            _controller.Should().NotBeNull();
            _controller.Should().BeOfType<OrderController>();
        }

        [Fact]
        public async Task PostAsync_WithValidOrderRequest_ShouldReturnActionResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IActionResult>();
        }

        [Fact]
        public async Task PostAsync_WithNullOrderRequest_ShouldHandleGracefully()
        {
            // Arrange
            OrderRequest nullOrder = null;

            // Act
            var result = await _controller.PostAsync(nullOrder);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IActionResult>();
        }

        [Fact]
        public void OrderController_ShouldInheritFromControllerBase()
        {
            // Act & Assert
            _controller.Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public async Task PostAsync_ShouldBeAsyncOperation()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var task = _controller.PostAsync(orderRequest);
            
            // Assert
            task.Should().NotBeNull();
            task.Should().BeAssignableTo<Task<IActionResult>>();
            
            var result = await task;
            result.Should().NotBeNull();
        }
    }
}