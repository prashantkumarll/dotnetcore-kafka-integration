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

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedAttributes()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }
        
        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var implementsDisposable = typeof(IDisposable).IsAssignableFrom(type);
            
            // Assert
            implementsDisposable.Should().BeTrue();
        }
        
        [Fact]
        public void ConsumerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var methods = type.GetMethods().Select(m => m.Name).ToArray();
            
            // Assert
            methods.Should().Contain("ReadMessageAsync");
            methods.Should().Contain("DisposeAsync");
            methods.Should().Contain("Dispose");
        }
        
        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptExpectedParameters()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var constructors = type.GetConstructors();
            var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);
            
            // Assert
            constructor.Should().NotBeNull();
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[1].ParameterType.Should().Be(typeof(string));
        }
    }
}