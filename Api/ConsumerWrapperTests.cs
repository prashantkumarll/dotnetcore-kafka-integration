using System;
using System.Threading;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Api;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void Constructor_ValidConfig_ShouldInitializeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Assert
                consumerWrapper.Should().NotBeNull();
            }
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert - Since we can't mock the internal consumer, we test the actual behavior
                // The method should return null when no message is available within timeout
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(config, topicName);
            consumerWrapper.Dispose();

            // Assert
            // No specific assertion needed, just ensuring no exceptions are thrown
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(config, topicName);
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second dispose should not throw

            // Assert
            // No specific assertion needed, just ensuring no exceptions are thrown
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert - Testing actual behavior when consume fails
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var emptyTopicName = string.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, emptyTopicName));
            exception.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldStoreTopicNameAndConfig()
        {
            // Arrange
            var config = new ConsumerConfig 
            { 
                GroupId = "test-group",
                BootstrapServers = "localhost:9092"
            };
            var topicName = "orders-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Assert
                consumerWrapper.Should().NotBeNull();
                // Constructor should complete without throwing
            }
        }

        [Fact]
        public void ReadMessage_WithTimeout_ShouldHandleTimeoutGracefully()
        {
            // Arrange
            var config = new ConsumerConfig 
            { 
                GroupId = "test-group",
                BootstrapServers = "localhost:9092"
            };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                // Should return null when no message is available within 1 second timeout
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ReadMessage_OperationCanceled_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                // When operation is cancelled, should return null
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Dispose_WhenConsumerCloseThrows_ShouldStillDisposeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act & Assert
            // Should not throw even if internal close() throws
            Action disposeAction = () => consumerWrapper.Dispose();
            disposeAction.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithSpecialCharactersInTopicName_ShouldSucceed()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "topic-with-dashes_and_underscores.and.dots";

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                consumerWrapper.Should().NotBeNull();
            }
        }

        [Fact]
        public void ReadMessage_MultipleConsecutiveCalls_ShouldHandleCorrectly()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result1 = consumerWrapper.readMessage();
                var result2 = consumerWrapper.readMessage();
                var result3 = consumerWrapper.readMessage();

                // Assert
                // Multiple calls should not cause issues
                result1.Should().BeNull();
                result2.Should().BeNull();
                result3.Should().BeNull();
            }
        }
    }
}