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
        public void ProcessOrdersService_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }
        
        [Fact]
        public void ProcessOrdersService_ShouldHaveCorrectConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act
            var constructors = type.GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(IConfiguration));
            
            // Assert
            targetConstructor.Should().NotBeNull();
            targetConstructor.GetParameters()[0].Name.Should().Be("configuration");
        }
    }
}