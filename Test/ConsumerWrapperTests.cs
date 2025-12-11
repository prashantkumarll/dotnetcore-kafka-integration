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
        public void ConsumerWrapper_Constructor_ShouldCreateInstance()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Theory]
        [InlineData("connection1", "topic1")]
        [InlineData("connection2", "topic2")]
        [InlineData("test-conn", "test-queue")]
        public void ConsumerWrapper_Constructor_WithDifferentParameters_ShouldCreateInstance(string connectionString, string topicName)
        {
            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedMethods()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var methods = consumerType.GetMethods().Where(m => m.DeclaringType == consumerType);

            // Assert
            methods.Should().Contain(m => m.Name == "ReadMessageAsync");
            methods.Should().Contain(m => m.Name == "DisposeAsync");
            methods.Should().Contain(m => m.Name == "Dispose");
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldHaveCorrectParameters()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var constructors = consumerType.GetConstructors();
            var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

            // Assert
            constructor.Should().NotBeNull();
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be<string>();
            parameters[1].ParameterType.Should().Be<string>();
            parameters[0].Name.Should().Be("connectionString");
            parameters[1].Name.Should().Be("topicName");
        }

        [Fact]
        public void ConsumerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            Action dispose = () => consumer.Dispose();
            dispose.Should().NotThrow();
        }

        [Fact]
        public void ConsumerWrapper_Namespace_ShouldBeCorrect()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var namespaceName = consumerType.Namespace;

            // Assert
            namespaceName.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_ShouldExist()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var readMessageMethod = consumerType.GetMethod("ReadMessageAsync");

            // Assert
            readMessageMethod.Should().NotBeNull();
            readMessageMethod.Name.Should().Be("ReadMessageAsync");
        }
    }
}