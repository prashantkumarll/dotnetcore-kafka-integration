using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Api;
using Api.Models;
using Api.Controllers;
using Api.Services;

namespace Test
{
    public class TypeValidationTests
    {
        [Theory]
        [InlineData(typeof(OrderRequest), "Api.Models")]
        [InlineData(typeof(OrderController), "Api.Controllers")]
        [InlineData(typeof(ProcessOrdersService), "Api.Services")]
        [InlineData(typeof(ProducerWrapper), "Api")]
        [InlineData(typeof(ConsumerWrapper), "Api")]
        [InlineData(typeof(Startup), "Api")]
        public void AllTypes_ShouldHaveCorrectNamespace(Type type, string expectedNamespace)
        {
            // Arrange & Act & Assert
            type.Namespace.Should().Be(expectedNamespace);
        }

        [Fact]
        public void AllApiTypes_ShouldBePublic()
        {
            // Arrange
            var types = new[]
            {
                typeof(OrderRequest),
                typeof(OrderController),
                typeof(ProcessOrdersService),
                typeof(ProducerWrapper),
                typeof(ConsumerWrapper),
                typeof(Startup)
            };

            // Act & Assert
            foreach (var type in types)
            {
                type.IsPublic.Should().BeTrue($"{type.Name} should be public");
            }
        }

        [Fact]
        public void OrderController_ShouldHaveServiceBusClientConstructor()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var constructors = type.GetConstructors();
            var serviceBusConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1);

            // Assert
            serviceBusConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveIConfigurationConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var constructors = type.GetConstructors();
            var configurationConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters().First().ParameterType == typeof(IConfiguration));

            // Assert
            configurationConstructor.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_ShouldHavePostAsyncMethod()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var methods = type.GetMethods();
            var postAsyncMethod = methods.FirstOrDefault(m => m.Name == "PostAsync");

            // Assert
            postAsyncMethod.Should().NotBeNull();
        }
    }
}