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
using Api;
using Api.Models;
using Api.Controllers;
using Api.Services;

namespace Test
{
    public class ApiNamespaceTests
    {
        [Fact]
        public void Api_Namespace_ShouldContainExpectedClasses()
        {
            // Arrange
            var apiAssembly = typeof(ProducerWrapper).Assembly;
            
            // Act
            var apiNamespaceTypes = apiAssembly.GetTypes()
                .Where(t => t.Namespace == "Api")
                .Select(t => t.Name)
                .ToList();
            
            // Assert
            apiNamespaceTypes.Should().NotBeEmpty();
            apiNamespaceTypes.Should().Contain("ProducerWrapper");
            apiNamespaceTypes.Should().Contain("ConsumerWrapper");
            apiNamespaceTypes.Should().Contain("Startup");
        }

        [Fact]
        public void Api_Models_Namespace_ShouldContainOrderRequest()
        {
            // Arrange
            var apiAssembly = typeof(OrderRequest).Assembly;
            
            // Act
            var modelsTypes = apiAssembly.GetTypes()
                .Where(t => t.Namespace == "Api.Models")
                .Select(t => t.Name)
                .ToList();
            
            // Assert
            modelsTypes.Should().NotBeEmpty();
            modelsTypes.Should().Contain("OrderRequest");
        }

        [Fact]
        public void Api_Controllers_Namespace_ShouldContainOrderController()
        {
            // Arrange
            var apiAssembly = typeof(OrderController).Assembly;
            
            // Act
            var controllerTypes = apiAssembly.GetTypes()
                .Where(t => t.Namespace == "Api.Controllers")
                .Select(t => t.Name)
                .ToList();
            
            // Assert
            controllerTypes.Should().NotBeEmpty();
            controllerTypes.Should().Contain("OrderController");
        }

        [Fact]
        public void Api_Services_Namespace_ShouldContainProcessOrdersService()
        {
            // Arrange
            var apiAssembly = typeof(ProcessOrdersService).Assembly;
            
            // Act
            var serviceTypes = apiAssembly.GetTypes()
                .Where(t => t.Namespace == "Api.Services")
                .Select(t => t.Name)
                .ToList();
            
            // Assert
            serviceTypes.Should().NotBeEmpty();
            serviceTypes.Should().Contain("ProcessOrdersService");
        }

        [Theory]
        [InlineData(typeof(ProducerWrapper), "Api")]
        [InlineData(typeof(ConsumerWrapper), "Api")]
        [InlineData(typeof(Startup), "Api")]
        [InlineData(typeof(OrderRequest), "Api.Models")]
        [InlineData(typeof(OrderController), "Api.Controllers")]
        [InlineData(typeof(ProcessOrdersService), "Api.Services")]
        public void Classes_ShouldHaveCorrectNamespace(Type type, string expectedNamespace)
        {
            // Act & Assert
            type.Namespace.Should().Be(expectedNamespace);
        }

        [Fact]
        public void All_Public_Classes_ShouldBeAccessible()
        {
            // Arrange
            var expectedTypes = new[]
            {
                typeof(ProducerWrapper),
                typeof(ConsumerWrapper),
                typeof(Startup),
                typeof(OrderRequest),
                typeof(OrderController),
                typeof(ProcessOrdersService)
            };
            
            // Act & Assert
            foreach (var type in expectedTypes)
            {
                type.Should().NotBeNull($"Type {type.Name} should be accessible");
                type.IsPublic.Should().BeTrue($"Type {type.Name} should be public");
            }
        }
    }
}