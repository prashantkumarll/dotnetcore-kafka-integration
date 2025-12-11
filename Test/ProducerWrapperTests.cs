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
        public void ProducerWrapper_Constructor_ShouldCreateInstance()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Theory]
        [InlineData("connection1", "topic1")]
        [InlineData("connection2", "topic2")]
        [InlineData("", "")]
        public void ProducerWrapper_Constructor_WithDifferentParameters_ShouldCreateInstance(string connectionString, string topicName)
        {
            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldHaveExpectedMethods()
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);

            // Act
            var methods = producerType.GetMethods().Where(m => m.DeclaringType == producerType);

            // Assert
            methods.Should().Contain(m => m.Name == "writeMessage");
            methods.Should().Contain(m => m.Name == "DisposeAsync");
            methods.Should().Contain(m => m.Name == "Dispose");
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldHaveCorrectParameters()
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);

            // Act
            var constructors = producerType.GetConstructors();
            var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

            // Assert
            constructor.Should().NotBeNull();
            var parameters = constructor.GetParameters();
            parameters[0].ParameterType.Should().Be<string>();
            parameters[1].ParameterType.Should().Be<string>();
            parameters[0].Name.Should().Be("connectionString");
            parameters[1].Name.Should().Be("topicName");
        }

        [Fact]
        public void ProducerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            Action dispose = () => producer.Dispose();
            dispose.Should().NotThrow();
        }

        [Fact]
        public void ProducerWrapper_Namespace_ShouldBeCorrect()
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);

            // Act
            var namespaceName = producerType.Namespace;

            // Assert
            namespaceName.Should().Be("Api");
        }
    }
}