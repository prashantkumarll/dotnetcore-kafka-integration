using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void Constructor_InvalidTopicName_EmptyString_ShouldThrowArgumentException()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ConsumerWrapper(mockConfig, string.Empty));
        }

        [Fact]
        public void Constructor_Subscribe_ShouldCallSubscribeOnConsumer()
        {
            // Arrange
            var mockConsumer = new Mock<IConsumer<string, string>>();
            var mockConfig = new ConsumerConfig();
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(mockConfig, topicName))
            {
                // Assert
                mockConsumer.Verify(c => c.Subscribe(topicName), Times.Once);
            }
        }

        [Fact]
        public void Dispose_ConsumerAlreadyClosed_ShouldNotThrowException()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(mockConfig, topicName))
            {
                consumerWrapper.Dispose();
                consumerWrapper.Dispose(); // Second dispose
            }

            // Assert - no exception means test passes
        }
    }
}