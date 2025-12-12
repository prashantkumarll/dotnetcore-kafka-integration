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
using Azure.Messaging.ServiceBus;

namespace Test
{
    public class OrderControllerTests
    {
        [Fact]
        public void OrderController_Type_ShouldHaveCorrectProperties()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("OrderController");
            type.Namespace.Should().Be("Api.Controllers");
        }

        [Fact]
        public void OrderController_Constructor_ShouldRequireServiceBusClient()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var constructors = type.GetConstructors();
            var mainConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);

            // Assert
            mainConstructor.Should().NotBeNull();
            var parameter = mainConstructor.GetParameters().First();
            parameter.ParameterType.Should().Be(typeof(ServiceBusClient));
        }

        [Fact]
        public void OrderController_PostAsyncMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var method = type.GetMethod("PostAsync");

            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Be("PostAsync");
        }

        [Fact]
        public void OrderController_Assembly_ShouldBeCorrect()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var assembly = type.Assembly;

            // Assert
            assembly.Should().NotBeNull();
            assembly.GetName().Name.Should().Be("Api");
        }

        [Fact]
        public void OrderController_Methods_ShouldBePublic()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var postAsyncMethod = type.GetMethod("PostAsync");

            // Assert
            postAsyncMethod.Should().NotBeNull();
            postAsyncMethod.IsPublic.Should().BeTrue();
        }
    }
}