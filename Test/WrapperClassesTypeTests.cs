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
    public class WrapperClassesTypeTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveCorrectStructure()
        {
            // Arrange & Act
            var type = typeof(ConsumerWrapper);

            // Assert
            type.Should().NotBeNull();
            type.Namespace.Should().Be("Api");
            type.Name.Should().Be("ConsumerWrapper");
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldHaveCorrectStructure()
        {
            // Arrange & Act
            var type = typeof(ProducerWrapper);

            // Assert
            type.Should().NotBeNull();
            type.Namespace.Should().Be("Api");
            type.Name.Should().Be("ProducerWrapper");
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var readMessageMethod = type.GetMethod("ReadMessageAsync");
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");
            var disposeMethod = type.GetMethod("Dispose");

            // Assert
            readMessageMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveExpectedMethods()
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
    }
}