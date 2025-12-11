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
        public void ConsumerWrapper_Type_ShouldHaveCorrectProperties()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var expectedConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters().All(p => p.ParameterType == typeof(string)));

            // Assert
            expectedConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Methods_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var methods = type.GetMethods().Select(m => m.Name).ToList();

            // Assert
            methods.Should().Contain("ReadMessageAsync");
            methods.Should().Contain("DisposeAsync");
            methods.Should().Contain("Dispose");
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
        public void ConsumerWrapper_ReadMessageAsync_ShouldBeAsyncMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var readMessageMethod = type.GetMethod("ReadMessageAsync");

            // Assert
            readMessageMethod.Should().NotBeNull();
            readMessageMethod.ReturnType.Name.Should().Contain("Task");
        }
    }
}