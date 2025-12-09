using System;
using System.Threading.Tasks;
using Api;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class ConsumerWrapperTests
    {
        private readonly string _mockConnectionString = "mock-connection-string";
        private readonly string _mockTopicName = "mock-topic";

        [Fact]
        public void ConsumerWrapper_ShouldBeCreated_WithConnectionStringAndTopic()
        {
            // Act
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var hasMethod = consumer.GetType().GetMethod("ReadMessageAsync") != null;

            // Assert
            hasMethod.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            consumer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public async Task ConsumerWrapper_ShouldDisposeAsync()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            await consumer.DisposeAsync();
        }

        [Fact]
        public void ConsumerWrapper_ShouldDispose()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            consumer.Dispose();
        }

        [Fact]
        public async Task ConsumerWrapper_ReadMessageAsync_ShouldBeAsynchronous()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var task = consumer.ReadMessageAsync();

            // Assert
            task.Should().NotBeNull();
            await task;
        }
    }
}