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
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            type.GetInterfaces().Should().Contain(typeof(IDisposable));
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveRequiredMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var methods = type.GetMethods().Select(m => m.Name);
            
            // Assert
            methods.Should().Contain("writeMessage");
            methods.Should().Contain("DisposeAsync");
            methods.Should().Contain("Dispose");
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveStringParameterConstructor()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(2);
            constructor.GetParameters().All(p => p.ParameterType == typeof(string)).Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_TypeInfo_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
            type.IsSealed.Should().BeFalse();
        }
    }
}