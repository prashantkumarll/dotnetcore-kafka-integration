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
    public class ServiceTypesTests
    {
        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveCorrectStructure()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProcessOrdersService");
            type.Namespace.Should().Be("Api.Services");
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveRequiredMethods()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_ShouldAcceptConfiguration()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            var constructors = type.GetConstructors();

            // Act
            var configConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(IConfiguration));

            // Assert
            configConstructor.Should().NotBeNull();
            configConstructor.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_ShouldImplementIDisposablePattern()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var implementsIDisposable = typeof(IDisposable).IsAssignableFrom(type);
            var implementsIAsyncDisposable = type.GetInterfaces().Any(i => i.Name == "IAsyncDisposable");

            // Assert
            implementsIDisposable.Should().BeTrue();
            implementsIAsyncDisposable.Should().BeTrue();
        }
    }
}