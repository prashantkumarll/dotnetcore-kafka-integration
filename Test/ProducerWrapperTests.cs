using System;
using System.Threading.Tasks;
using Api;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests
    {
        private readonly string _mockConnectionString = "mock-connection-string";
        private readonly string _mockTopicName = "mock-topic";

        [Fact]
        public void ProducerWrapper_ShouldBeCreated_WithConnectionStringAndTopic()
        {
            // Act
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var hasMethod = producer.GetType().GetMethod("writeMessage") != null;

            // Assert
            hasMethod.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            producer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public async Task ProducerWrapper_ShouldDisposeAsync()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            await producer.DisposeAsync();
        }

        [Fact]
        public void ProducerWrapper_ShouldDispose()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            producer.Dispose();
        }
    }
}