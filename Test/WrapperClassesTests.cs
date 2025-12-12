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
        public void ProducerWrapper_Constructor_ShouldRequireTwoStringParameters()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var mainConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

            // Assert
            mainConstructor.Should().NotBeNull();
            var parameters = mainConstructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters.All(p => p.ParameterType == typeof(string)).Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var writeMessageMethod = type.GetMethod("writeMessage");
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            writeMessageMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

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
        public void ConsumerWrapper_Constructor_ShouldRequireTwoStringParameters()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var mainConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

            // Assert
            mainConstructor.Should().NotBeNull();
            var parameters = mainConstructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters.All(p => p.ParameterType == typeof(string)).Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Methods_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var readMessageAsyncMethod = type.GetMethod("ReadMessageAsync");
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            readMessageAsyncMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }
    }
}