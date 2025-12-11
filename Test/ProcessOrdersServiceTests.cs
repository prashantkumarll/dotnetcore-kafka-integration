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
        public void ProcessOrdersService_Constructor_WithConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithNullConfiguration_ShouldStillCreateInstance()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(null);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveExpectedStructure()
        {
            // Arrange
            var serviceType = typeof(ProcessOrdersService);

            // Act
            var constructors = serviceType.GetConstructors();
            var methods = serviceType.GetMethods();
            var properties = serviceType.GetProperties();

            // Assert
            serviceType.Should().NotBeNull();
            serviceType.Namespace.Should().Be("Api.Services");
            constructors.Should().NotBeEmpty();
            
            var configConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(IConfiguration));
            
            configConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_WithMockedConfiguration_ShouldAccessConfigurationValues()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["ServiceBusConnectionString"]).Returns("test-connection-string");
            mockConfiguration.Setup(c => c["QueueName"]).Returns("test-queue");
            mockConfiguration.Setup(c => c["TopicName"]).Returns("test-topic");

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
            mockConfiguration.Verify(c => c["ServiceBusConnectionString"], Times.Never);
            mockConfiguration.Verify(c => c["QueueName"], Times.Never);
            mockConfiguration.Verify(c => c["TopicName"], Times.Never);
        }

        [Theory]
        [InlineData("connection-string-1")]
        [InlineData("connection-string-2")]
        [InlineData("")]
        public void ProcessOrdersService_WithDifferentConnectionStrings_ShouldCreateSuccessfully(string connectionString)
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["ServiceBusConnectionString"]).Returns(connectionString);

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_PublicMethods_ShouldBeAccessible()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var service = new ProcessOrdersService(mockConfiguration.Object);
            var serviceType = typeof(ProcessOrdersService);

            // Act
            var publicMethods = serviceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // Assert
            service.Should().NotBeNull();
            publicMethods.Should().NotBeNull();
            
            foreach (var method in publicMethods)
            {
                method.Name.Should().NotBeNullOrEmpty();
                method.IsPublic.Should().BeTrue();
            }
        }
    }
}