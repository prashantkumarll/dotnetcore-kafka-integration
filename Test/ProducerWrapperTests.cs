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
        public void ProducerWrapper_Type_ShouldHaveCorrectProperties()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var expectedConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters().All(p => p.ParameterType == typeof(string)));

            // Assert
            expectedConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var methods = type.GetMethods().Select(m => m.Name).ToList();

            // Assert
            methods.Should().Contain("writeMessage");
            methods.Should().Contain("DisposeAsync");
            methods.Should().Contain("Dispose");
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var interfaces = type.GetInterfaces();

            // Assert
            interfaces.Should().Contain(typeof(IDisposable));
        }
    }
}