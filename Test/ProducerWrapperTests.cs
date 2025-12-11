using Xunit;
using FluentAssertions;
using Api;
using System;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_ShouldBeInstantiated_WithValidParameters()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            producer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ProducerWrapper_Dispose_ShouldNotThrowException()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            producer.Invoking(p => p.Dispose()).Should().NotThrow();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsync_ShouldNotThrowException()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act & Assert
            await producer.Invoking(async p => await p.DisposeAsync()).Should().NotThrowAsync();
        }

        [Fact]
        public async Task ProducerWrapper_WriteMessage_ShouldNotThrowWithValidMessage()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);
            var message = "test message";

            // Act & Assert
            await producer.Invoking(async p => await p.writeMessage(message)).Should().NotThrowAsync();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ProducerWrapper_ShouldHandleEmptyConnectionString(string connectionString)
        {
            // Arrange
            var topicName = "test-topic";

            // Act & Assert
            var producer = new ProducerWrapper(connectionString, topicName);
            producer.Should().NotBeNull();
        }
    }
}