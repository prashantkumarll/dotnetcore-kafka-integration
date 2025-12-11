using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Api;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(ConsumerWrapper);

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveConstructorWithTwoStringParameters()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var twoStringConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters().All(p => p.ParameterType == typeof(string)));

            // Assert
            twoStringConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var methods = type.GetMethods();
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
            var type = typeof(ConsumerWrapper);

            // Act
            var interfaces = type.GetInterfaces();

            // Assert
            interfaces.Should().Contain(typeof(IDisposable));
        }

        [Fact]
        public void ConsumerWrapper_TypeInfo_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }
    }
}