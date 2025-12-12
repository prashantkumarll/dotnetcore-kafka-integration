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
using Api.Services;
using Api.Controllers;

namespace Test
{
    public class ReflectionBasedTests
    {
        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveExpectedStructure()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProcessOrdersService");
            type.Namespace.Should().Be("Api.Services");
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderController_Type_ShouldHaveExpectedStructure()
        {
            // Arrange
            var type = typeof(OrderController);

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("OrderController");
            type.Namespace.Should().Be("Api.Controllers");
            type.IsClass.Should().BeTrue();
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

        [Fact]
        public void AllApiClasses_ShouldHavePublicConstructors()
        {
            // Arrange
            var apiTypes = new[] 
            {
                typeof(ProducerWrapper),
                typeof(ConsumerWrapper),
                typeof(Startup),
                typeof(ProcessOrdersService),
                typeof(OrderController),
                typeof(OrderRequest)
            };

            // Act & Assert
            foreach (var type in apiTypes)
            {
                var constructors = type.GetConstructors();
                constructors.Should().NotBeEmpty($"Type {type.Name} should have public constructors");
                constructors.Should().Contain(c => c.IsPublic, $"Type {type.Name} should have at least one public constructor");
            }
        }

        [Fact]
        public void AllApiClasses_ShouldBeInCorrectNamespaces()
        {
            // Arrange & Act & Assert
            typeof(ProducerWrapper).Namespace.Should().Be("Api");
            typeof(ConsumerWrapper).Namespace.Should().Be("Api");
            typeof(Startup).Namespace.Should().Be("Api");
            typeof(ProcessOrdersService).Namespace.Should().Be("Api.Services");
            typeof(OrderController).Namespace.Should().Be("Api.Controllers");
            typeof(OrderRequest).Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void ConfigurationMocking_ShouldWorkCorrectly()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["TestSetting"]).Returns("TestValue");

            // Act
            var value = mockConfig.Object["TestSetting"];

            // Assert
            value.Should().Be("TestValue");
            mockConfig.Verify(c => c["TestSetting"], Times.Once);
        }

        [Theory]
        [InlineData("Api")]
        [InlineData("Api.Models")]
        [InlineData("Api.Services")]
        [InlineData("Api.Controllers")]
        public void Namespaces_ShouldExist(string expectedNamespace)
        {
            // Arrange
            var assembly = typeof(ProducerWrapper).Assembly;

            // Act
            var typesInNamespace = assembly.GetTypes()
                .Where(t => t.Namespace == expectedNamespace)
                .ToList();

            // Assert
            typesInNamespace.Should().NotBeEmpty($"Namespace {expectedNamespace} should contain types");
        }
    }
}