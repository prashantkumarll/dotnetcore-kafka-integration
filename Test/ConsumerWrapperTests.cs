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
        public void ConsumerWrapper_Type_ShouldHaveCorrectNamespace()
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
            
            // Act & Assert
            type.GetInterfaces().Should().Contain(typeof(IDisposable));
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveRequiredMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var methods = type.GetMethods().Select(m => m.Name);
            
            // Assert
            methods.Should().Contain("ReadMessageAsync");
            methods.Should().Contain("DisposeAsync");
            methods.Should().Contain("Dispose");
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveStringParameterConstructor()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(2);
            constructor.GetParameters().All(p => p.ParameterType == typeof(string)).Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_TypeInfo_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
            type.IsSealed.Should().BeFalse();
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("ReadMessageAsync");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
    }
}