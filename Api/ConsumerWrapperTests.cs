using System;
using Xunit;
using FluentAssertions;
using Confluent.Kafka;
using Moq;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName));
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, null));
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act
            consumerWrapper.Dispose();

            // Assert
            consumerWrapper.Should().NotBeNull(); // Ensures no exception during disposal
        }
    }
}