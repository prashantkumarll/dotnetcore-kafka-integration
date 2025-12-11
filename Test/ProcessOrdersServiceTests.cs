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
        public void ProcessOrdersService_ShouldHaveConfigurationConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(1);
            constructor.GetParameters().First().ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void ProcessOrdersService_TypeInfo_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
            type.IsSealed.Should().BeFalse();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHavePublicConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act
            var constructors = type.GetConstructors();
            var publicConstructors = constructors.Where(c => c.IsPublic);
            
            // Assert
            constructors.Should().NotBeEmpty();
            publicConstructors.Should().NotBeEmpty();
        }

        [Fact]
        public void ProcessOrdersService_ConstructorParameter_ShouldBeIConfiguration()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            var constructor = type.GetConstructors().First();
            
            // Act
            var parameters = constructor.GetParameters();
            
            // Assert
            parameters.Should().HaveCount(1);
            parameters.First().ParameterType.Should().Be(typeof(IConfiguration));
            parameters.First().Name.Should().Be("configuration");
        }
    }
}