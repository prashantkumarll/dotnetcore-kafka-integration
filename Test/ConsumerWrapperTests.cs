using Xunit;
using FluentAssertions;
using Api;
using System;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_ShouldBeInstantiated_WithValidParameters()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            consumer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public async Task ConsumerWrapper_ReadMessageAsync_ShouldReturnTask()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act
            var result = consumer.ReadMessageAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task>();
        }

        [Fact]
        public void ConsumerWrapper_Dispose_ShouldNotThrowException()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            consumer.Invoking(c => c.Dispose()).Should().NotThrow();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsync_ShouldNotThrowException()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            await consumer.Invoking(async c => await c.DisposeAsync()).Should().NotThrowAsync();
        }

        [Theory]
        [InlineData("test-connection", "topic1")]
        [InlineData("another-connection", "topic2")]
        public void ConsumerWrapper_ShouldAcceptDifferentParameters(string connectionString, string topicName)
        {
            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHandleEmptyTopicName()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().NotBeNull();
        }
    }
}