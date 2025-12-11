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
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var methods = type.GetMethods().Where(m => m.DeclaringType == type).ToList();

            // Assert
            methods.Should().NotBeEmpty();
            methods.Should().Contain(m => m.Name == "ReadMessageAsync");
            methods.Should().Contain(m => m.Name == "DisposeAsync");
            methods.Should().Contain(m => m.Name == "Dispose");
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveConstructorWithTwoStringParameters()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });

            // Assert
            constructor.Should().NotBeNull();
            constructor.IsPublic.Should().BeTrue();
            constructor.GetParameters().Should().HaveCount(2);
            constructor.GetParameters().All(p => p.ParameterType == typeof(string)).Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_MethodShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var readMessageAsyncMethod = type.GetMethod("ReadMessageAsync");

            // Assert
            readMessageAsyncMethod.Should().NotBeNull();
            readMessageAsyncMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_DisposeAsync_MethodShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");

            // Assert
            disposeAsyncMethod.Should().NotBeNull();
            disposeAsyncMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Dispose_MethodShouldExist()
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
        public void ConsumerWrapper_ShouldImplementDisposablePattern()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var interfaces = type.GetInterfaces();

            // Assert
            interfaces.Should().Contain(i => i.Name.Contains("Disposable"));
        }
    }
}