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
using Api.Services;
using Api.Controllers;

namespace Test
{
    public class ServiceTypesTests
    {
        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveCorrectStructure()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Should().NotBeNull();
            type.Namespace.Should().Be("Api.Services");
            type.Name.Should().Be("ProcessOrdersService");
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderController_Type_ShouldHaveCorrectStructure()
        {
            // Arrange & Act
            var type = typeof(OrderController);

            // Assert
            type.Should().NotBeNull();
            type.Namespace.Should().Be("Api.Controllers");
            type.Name.Should().Be("OrderController");
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderController_ShouldHavePostAsyncMethod()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var postAsyncMethod = type.GetMethod("PostAsync");

            // Assert
            postAsyncMethod.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveConstructorWithConfiguration()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void AllServiceTypes_ShouldBePublic()
        {
            // Arrange
            var serviceTypes = new[] { typeof(ProcessOrdersService), typeof(OrderController) };

            // Act & Assert
            foreach (var type in serviceTypes)
            {
                type.IsPublic.Should().BeTrue($"because {type.Name} should be public");
            }
        }
    }
}