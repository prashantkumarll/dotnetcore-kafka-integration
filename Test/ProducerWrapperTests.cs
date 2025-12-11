using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Api;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Type_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var methods = type.GetMethods().Where(m => m.DeclaringType == type).ToList();

            // Assert
            methods.Should().NotBeEmpty();
            methods.Should().Contain(m => m.Name == "writeMessage");
            methods.Should().Contain(m => m.Name == "DisposeAsync");
            methods.Should().Contain(m => m.Name == "Dispose");
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveConstructorWithTwoStringParameters()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });

            // Assert
            constructor.Should().NotBeNull();
            constructor.IsPublic.Should().BeTrue();
            constructor.GetParameters().Should().HaveCount(2);
            constructor.GetParameters().All(p => p.ParameterType == typeof(string)).Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var interfaces = type.GetInterfaces();

            // Assert
            interfaces.Should().Contain(i => i.Name.Contains("Disposable"));
        }

        [Fact]
        public void ProducerWrapper_WriteMessage_MethodShouldExist()
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
        public void ProducerWrapper_DisposeAsync_MethodShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");

            // Assert
            disposeAsyncMethod.Should().NotBeNull();
            disposeAsyncMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Dispose_MethodShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            disposeMethod.Should().NotBeNull();
            disposeMethod.IsPublic.Should().BeTrue();
        }
    }
}