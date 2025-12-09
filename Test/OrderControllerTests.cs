using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers;
using Api.Models;
using Api;
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
        public void OrderController_Should_BeInstantiable()
        {
            // Arrange & Act
            var controller = new OrderController();

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_Should_HavePostAsyncMethod()
        {
            // Arrange & Act
            var method = typeof(OrderController).GetMethod("PostAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_Should_ReturnActionResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_With_ValidOrderRequest_Should_NotThrow()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act & Assert
            await FluentActions.Invoking(async () => await _controller.PostAsync(orderRequest))
                .Should().NotThrowAsync();
        }

        [Fact]
        public void OrderController_Should_BeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(OrderController);

            // Assert
            type.Namespace.Should().Be("Api.Controllers");
        }
    }
}