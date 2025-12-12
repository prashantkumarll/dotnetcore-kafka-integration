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
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_TypeInfo_ShouldBeCorrect()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var methods = type.GetMethods().Where(m => m.DeclaringType == type);
            var methodNames = methods.Select(m => m.Name).ToList();

            // Assert
            methodNames.Should().Contain("writeMessage");
            methodNames.Should().Contain("DisposeAsync");
            methodNames.Should().Contain("Dispose");
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var parameterTypes = constructors.FirstOrDefault()?.GetParameters();

            // Assert
            constructors.Should().NotBeEmpty();
            parameterTypes.Should().HaveCount(2);
            parameterTypes?[0].ParameterType.Should().Be(typeof(string));
            parameterTypes?[1].ParameterType.Should().Be(typeof(string));
        }
    }
}