using Xunit;
using Api;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptConnectionStringAndTopicName()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic";

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
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            consumer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act
            var methodInfo = consumer.GetType().GetMethod("ReadMessageAsync");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsync_ShouldNotThrow()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            await FluentActions.Invoking(async () => await consumer.DisposeAsync())
                .Should().NotThrowAsync();
        }

        [Fact]
        public void ConsumerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act & Assert
            FluentActions.Invoking(() => consumer.Dispose())
                .Should().NotThrow();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_WithEmptyConnectionString_ShouldNotThrow()
        {
            // Arrange
            var connectionString = "";
            var topicName = "test-topic";

            // Act & Assert
            FluentActions.Invoking(() => new ConsumerWrapper(connectionString, topicName))
                .Should().NotThrow();
        }
    }
}