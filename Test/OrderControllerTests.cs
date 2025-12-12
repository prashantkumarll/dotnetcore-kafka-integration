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
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            
            // Act
            var controller = new OrderController(producerConfig);
            
            // Assert
            controller.Should().NotBeNull();
            controller.Should().BeOfType<OrderController>();
        }

        [Fact]
        public void OrderController_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(OrderController);
            
            // Act & Assert
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
            var postAsyncMethod = type.GetMethods().FirstOrDefault(m => m.Name == "PostAsync");
            
            // Assert
            postAsyncMethod.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_Constructor_ShouldAcceptProducerConfig()
        {
            // Arrange
            var type = typeof(OrderController);
            
            // Act
            var constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 1);
            
            // Assert
            constructor.Should().NotBeNull();
            var parameter = constructor.GetParameters().First();
            parameter.ParameterType.Should().Be(typeof(ProducerConfig));
        }
    }
}