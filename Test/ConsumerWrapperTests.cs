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

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedAttributes()
        {
            // Arrange & Act
            var type = typeof(Api.ConsumerWrapper);
            
            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange
            var type = typeof(Api.ConsumerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(2);
            constructor.GetParameters().First().ParameterType.Should().Be(typeof(string));
            constructor.GetParameters().Last().ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(Api.ConsumerWrapper);
            
            // Act
            var methods = type.GetMethods().Where(m => m.IsPublic && m.DeclaringType == type);
            var methodNames = methods.Select(m => m.Name).ToList();
            
            // Assert
            methodNames.Should().Contain("ReadMessageAsync");
            methodNames.Should().Contain("DisposeAsync");
            methodNames.Should().Contain("Dispose");
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(Api.ConsumerWrapper);
            
            // Act
            var interfaces = type.GetInterfaces();
            
            // Assert
            interfaces.Should().Contain(typeof(IDisposable));
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_ShouldBeAsyncMethod()
        {
            // Arrange
            var type = typeof(Api.ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("ReadMessageAsync");
            
            // Assert
            method.Should().NotBeNull();
            method.Name.Should().EndWith("Async");
        }
    }
}