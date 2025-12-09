using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Api;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptConnectionStringAndTopicName()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act
            var method = producer.GetType().GetMethod("writeMessage");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act
            var method = producer.GetType().GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act
            var method = producer.GetType().GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.GetType().Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Constructor_WithNullConnectionString_ShouldNotThrow()
        {
            // Arrange
            string connectionString = null;
            var topicName = "mock-topic-name";

            // Act
            Action act = () => new ProducerWrapper(connectionString, topicName);

            // Assert
            act.Should().NotThrow();
        }
    }
}