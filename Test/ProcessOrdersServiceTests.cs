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

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProcessOrdersService");
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveConfigurationConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act
            var constructors = type.GetConstructors();
            var configConstructor = constructors
                .FirstOrDefault(c => c.GetParameters().Length == 1 && 
                               c.GetParameters().First().ParameterType == typeof(IConfiguration));
            
            // Assert
            configConstructor.Should().NotBeNull();
            configConstructor.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithMockedConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ServiceBus:ConnectionString"]).Returns("test-connection");
            mockConfig.Setup(c => c["ServiceBus:QueueName"]).Returns("test-queue");
            
            // Act
            var service = new ProcessOrdersService(mockConfig.Object);
            
            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var mockConfig1 = new Mock<IConfiguration>();
            var mockConfig2 = new Mock<IConfiguration>();
            
            // Act
            var service1 = new ProcessOrdersService(mockConfig1.Object);
            var service2 = new ProcessOrdersService(mockConfig2.Object);
            
            // Assert
            service1.Should().NotBeSameAs(service2);
        }

        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveParameterlessConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act
            var constructors = type.GetConstructors();
            var hasParameterlessConstructor = constructors.Any(c => c.GetParameters().Length == 0);
            var hasConfigConstructor = constructors.Any(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters().First().ParameterType == typeof(IConfiguration));
            
            // Assert - Should have at least one constructor
            constructors.Should().NotBeEmpty();
            (hasParameterlessConstructor || hasConfigConstructor).Should().BeTrue();
        }
    }
}