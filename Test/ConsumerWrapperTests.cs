using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Api;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptConnectionStringAndTopicName()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act
            var method = consumer.GetType().GetMethod("ReadMessageAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act
            var method = consumer.GetType().GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act
            var method = consumer.GetType().GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.GetType().Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Constructor_WithEmptyTopicName_ShouldNotThrow()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "";

            // Act
            Action act = () => new ConsumerWrapper(connectionString, topicName);

            // Assert
            act.Should().NotThrow();
        }
    }
}