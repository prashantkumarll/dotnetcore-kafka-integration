using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Api;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Type_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(ProducerWrapper);

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveConstructorWithTwoStringParameters()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var twoStringConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters().All(p => p.ParameterType == typeof(string)));

            // Assert
            twoStringConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var methods = type.GetMethods();
            var methodNames = methods.Select(m => m.Name).ToList();

            // Assert
            methodNames.Should().Contain("writeMessage");
            methodNames.Should().Contain("DisposeAsync");
            methodNames.Should().Contain("Dispose");
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

        [Fact]
        public void ProducerWrapper_TypeInfo_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }
    }
}