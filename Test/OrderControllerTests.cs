using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Api.Controllers;

namespace Test
{
    public class OrderControllerTests
    {
        [Fact]
        public void OrderController_Type_ShouldExist()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("OrderController");
            type.Namespace.Should().Be("Api.Controllers");
        }

        [Fact]
        public void OrderController_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            isPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_PostAsyncMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var methods = type.GetMethods();
            var postAsyncMethod = methods.FirstOrDefault(m => m.Name == "PostAsync");

            // Assert
            postAsyncMethod.Should().NotBeNull();
            postAsyncMethod?.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_Constructor_ShouldExist()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var constructors = type.GetConstructors();

            // Assert
            constructors.Should().NotBeNull();
            constructors.Should().NotBeEmpty();
        }

        [Fact]
        public void OrderController_Type_ShouldNotBeAbstract()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var isAbstract = type.IsAbstract;

            // Assert
            isAbstract.Should().BeFalse();
        }

        [Fact]
        public void OrderController_Type_ShouldNotBeInterface()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var isInterface = type.IsInterface;

            // Assert
            isInterface.Should().BeFalse();
        }

        [Fact]
        public void OrderController_Methods_ShouldNotBeStatic()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var postAsyncMethod = type.GetMethod("PostAsync");

            // Assert
            postAsyncMethod?.IsStatic.Should().BeFalse();
        }

        [Fact]
        public void OrderController_Assembly_ShouldBeAccessible()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var assembly = type.Assembly;

            // Assert
            assembly.Should().NotBeNull();
            assembly.GetTypes().Should().Contain(type);
        }
    }
}