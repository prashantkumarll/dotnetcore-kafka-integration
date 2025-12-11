// REQUIRED using statements - COPY THIS BLOCK TO EVERY TEST FILE:
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
    public class AssemblyTests
    {
        [Fact]
        public void Api_Assembly_ShouldLoadCorrectly()
        {
            // Arrange
            var assembly = typeof(OrderRequest).Assembly;
            
            // Act & Assert
            assembly.Should().NotBeNull();
            assembly.GetName().Name.Should().Be("Api");
        }

        [Fact]
        public void Api_Assembly_ShouldContainExpectedTypes()
        {
            // Arrange
            var assembly = typeof(OrderRequest).Assembly;
            
            // Act
            var types = assembly.GetTypes().Select(t => t.Name).ToList();
            
            // Assert
            types.Should().Contain("OrderRequest");
            types.Should().Contain("OrderController");
            types.Should().Contain("ProcessOrdersService");
            types.Should().Contain("ProducerWrapper");
            types.Should().Contain("ConsumerWrapper");
            types.Should().Contain("Startup");
        }

        [Fact]
        public void Api_Models_Namespace_ShouldContainOrderRequest()
        {
            // Arrange
            var assembly = typeof(OrderRequest).Assembly;
            
            // Act
            var modelsTypes = assembly.GetTypes()
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
            var assembly = typeof(OrderRequest).Assembly;
            
            // Act
            var controllerTypes = assembly.GetTypes()
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
            var assembly = typeof(OrderRequest).Assembly;
            
            // Act
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.Namespace == "Api.Services")
                .Select(t => t.Name)
                .ToList();
            
            // Assert
            serviceTypes.Should().NotBeEmpty();
            serviceTypes.Should().Contain("ProcessOrdersService");
        }

        [Theory]
        [InlineData("OrderRequest", "Api.Models")]
        [InlineData("OrderController", "Api.Controllers")]
        [InlineData("ProcessOrdersService", "Api.Services")]
        [InlineData("ProducerWrapper", "Api")]
        [InlineData("ConsumerWrapper", "Api")]
        [InlineData("Startup", "Api")]
        public void Api_Types_ShouldHaveCorrectNamespace(string typeName, string expectedNamespace)
        {
            // Arrange
            var assembly = typeof(OrderRequest).Assembly;
            
            // Act
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            
            // Assert
            type.Should().NotBeNull($"{typeName} should exist in the assembly");
            type.Namespace.Should().Be(expectedNamespace);
        }
    }
}