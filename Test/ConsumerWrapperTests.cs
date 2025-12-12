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
            type.Namespace.Should().Be("Api");
            type.Name.Should().Be("ConsumerWrapper");
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var implementsDisposable = typeof(IDisposable).IsAssignableFrom(type);

            // Assert
            implementsDisposable.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Should_HaveReadMessageMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var readMessageMethod = type.GetMethod("readMessage");

            // Assert
            readMessageMethod.Should().NotBeNull();
            readMessageMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Should_HaveDisposeMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            disposeMethod.Should().NotBeNull();
            disposeMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

            // Assert
            constructors.Should().NotBeEmpty();
            targetConstructor.Should().NotBeNull();
            targetConstructor.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Assembly_ShouldBeCorrect()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var assembly = type.Assembly;

            // Assert
            assembly.Should().NotBeNull();
            assembly.GetName().Name.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Methods_ShouldHaveCorrectSignatures()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var readMessageMethod = type.GetMethod("readMessage");
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            readMessageMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
            readMessageMethod.ReturnType.Should().NotBeNull();
            disposeMethod.ReturnType.Should().Be(typeof(void));
        }
    }
}