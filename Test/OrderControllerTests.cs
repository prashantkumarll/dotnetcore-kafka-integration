using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Api.Controllers;
using Confluent.Kafka;

namespace Test
{
    public class OrderControllerTests
    {
        [Fact]
        public void OrderController_Constructor_ShouldCreateInstance()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "test-server" };

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
            controller.Should().BeOfType<OrderController>();
        }

        [Fact]
        public void OrderController_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(OrderController);

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("OrderController");
            type.Namespace.Should().Be("Api.Controllers");
        }

        [Fact]
        public void OrderController_ShouldHavePostAsyncMethod()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var method = type.GetMethod("PostAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
    }
}