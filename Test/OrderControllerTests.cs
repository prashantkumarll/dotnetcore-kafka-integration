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
        public void OrderController_Type_ShouldHaveExpectedProperties()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("OrderController");
            type.Namespace.Should().Be("Api.Controllers");
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_Constructor_ShouldAcceptServiceBusClientParameter()
        {
            // Arrange
            var constructors = typeof(OrderController).GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);

            // Act & Assert
            targetConstructor.Should().NotBeNull();
            targetConstructor.GetParameters().First().ParameterType.Should().Be(typeof(ServiceBusClient));
        }

        [Fact]
        public void OrderController_PublicMethods_ShouldContainPostAsync()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var methodNames = methods.Select(m => m.Name).ToList();

            // Assert
            methods.Should().NotBeEmpty();
            methodNames.Should().Contain("PostAsync");
        }

        [Fact]
        public void OrderController_Assembly_ShouldMatchExpected()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var assembly = type.Assembly;

            // Assert
            assembly.Should().NotBeNull();
            assembly.GetName().Name.Should().Be("Api");
        }
    }
}