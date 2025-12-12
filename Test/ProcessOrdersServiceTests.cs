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
using Confluent.Kafka;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
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
        public void ProcessOrdersService_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
        }
        
        [Fact]
        public void ProcessOrdersService_ShouldHaveExpectedConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act
            var constructors = type.GetConstructors();
            var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);
            
            // Assert
            constructor.Should().NotBeNull();
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(2);
        }
    }
}