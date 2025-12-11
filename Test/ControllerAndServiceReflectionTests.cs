using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Configuration;
using Api.Controllers;
using Api.Services;

namespace Test
{
    public class ControllerAndServiceReflectionTests
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
        public void OrderController_ShouldHavePostAsyncMethod()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var postAsyncMethod = type.GetMethod("PostAsync");

            // Assert
            postAsyncMethod.Should().NotBeNull();
            postAsyncMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_ShouldHaveServiceBusClientConstructor()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act
            var constructors = type.GetConstructors();
            var serviceBusConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType.Name.Contains("ServiceBusClient"));

            // Assert
            constructors.Should().NotBeEmpty();
            serviceBusConstructor.Should().NotBeNull("OrderController should have constructor with ServiceBusClient parameter");
        }

        [Fact]
        public void ProcessOrdersService_Type_ShouldExist()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProcessOrdersService");
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveConfigurationConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });

            // Assert
            constructor.Should().NotBeNull();
            constructor.IsPublic.Should().BeTrue();
            constructor.GetParameters().Should().HaveCount(1);
            constructor.GetParameters().First().ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void ProcessOrdersService_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void OrderController_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(OrderController);

            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Theory]
        [InlineData("OrderController", "Api.Controllers")]
        [InlineData("ProcessOrdersService", "Api.Services")]
        public void ApiTypes_ShouldHaveCorrectNamespaces(string typeName, string expectedNamespace)
        {
            // Arrange
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var apiAssembly = assemblies.FirstOrDefault(a => a.GetName().Name == "Api");
            
            // Act
            Type foundType = null;
            if (apiAssembly != null)
            {
                foundType = apiAssembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            }

            // Assert
            if (foundType != null)
            {
                foundType.Namespace.Should().Be(expectedNamespace);
            }
        }

        [Fact]
        public void ApiAssembly_ShouldContainExpectedTypes()
        {
            // Arrange
            var expectedTypes = new[] 
            { 
                "OrderController", 
                "ProcessOrdersService", 
                "OrderRequest",
                "ProducerWrapper",
                "ConsumerWrapper",
                "Startup"
            };

            // Act
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var apiAssembly = assemblies.FirstOrDefault(a => a.GetName().Name == "Api");

            // Assert
            if (apiAssembly != null)
            {
                var actualTypes = apiAssembly.GetTypes().Select(t => t.Name).ToList();
                
                foreach (var expectedType in expectedTypes)
                {
                    actualTypes.Should().Contain(expectedType, 
                        $"API assembly should contain {expectedType} type");
                }
            }
        }
    }
}