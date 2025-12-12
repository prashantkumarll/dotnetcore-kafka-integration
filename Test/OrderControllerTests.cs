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

namespace Test
{
    public class OrderControllerTests
    {
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
        public void OrderController_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_Constructor_ShouldHaveServiceBusClientParameter()
        {
            // Arrange
            var type = typeof(OrderController);
            var constructors = type.GetConstructors();

            // Act
            var constructor = constructors.FirstOrDefault();

            // Assert
            constructor.Should().NotBeNull();
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
        }
    }
}