using Xunit;
using Api;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptConnectionStringAndTopicName()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic";

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
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            producer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act
            var methodInfo = producer.GetType().GetMethod("writeMessage");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsync_ShouldNotThrow()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            await FluentActions.Invoking(async () => await producer.DisposeAsync())
                .Should().NotThrowAsync();
        }

        [Fact]
        public void ProducerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            FluentActions.Invoking(() => producer.Dispose())
                .Should().NotThrow();
        }
    }
}