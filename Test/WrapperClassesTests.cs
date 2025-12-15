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
    public class WrapperClassesTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedProperties()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange
            var constructors = typeof(ConsumerWrapper).GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

            // Act & Assert
            targetConstructor.Should().NotBeNull();
            var parameters = targetConstructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[1].ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ConsumerWrapper_PublicMethods_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var methodNames = methods.Select(m => m.Name).ToList();

            // Assert
            methodNames.Should().Contain("ReadMessageAsync");
            methodNames.Should().Contain("DisposeAsync");
            methodNames.Should().Contain("Dispose");
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldHaveExpectedProperties()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange
            var constructors = typeof(ProducerWrapper).GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

            // Act & Assert
            targetConstructor.Should().NotBeNull();
            var parameters = targetConstructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[1].ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ProducerWrapper_PublicMethods_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var methodNames = methods.Select(m => m.Name).ToList();

            // Assert
            methodNames.Should().Contain("writeMessage");
            methodNames.Should().Contain("DisposeAsync");
            methodNames.Should().Contain("Dispose");
        }
    }
}