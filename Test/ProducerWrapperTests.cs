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
            type.Namespace.Should().Be("Api");
            type.Name.Should().Be("ProducerWrapper");
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var implementsDisposable = typeof(IDisposable).IsAssignableFrom(type);

            // Assert
            implementsDisposable.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Should_HaveWriteMessageMethod()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var writeMessageMethod = type.GetMethod("writeMessage");

            // Assert
            writeMessageMethod.Should().NotBeNull();
            writeMessageMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Should_HaveDisposeMethod()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            disposeMethod.Should().NotBeNull();
            disposeMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

            // Assert
            constructors.Should().NotBeEmpty();
            targetConstructor.Should().NotBeNull();
            targetConstructor.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Assembly_ShouldBeCorrect()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var assembly = type.Assembly;

            // Assert
            assembly.Should().NotBeNull();
            assembly.GetName().Name.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldHaveCorrectSignatures()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var writeMessageMethod = type.GetMethod("writeMessage");
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            writeMessageMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
            writeMessageMethod.ReturnType.Should().NotBeNull();
            disposeMethod.ReturnType.Should().Be(typeof(void));
        }
    }
}