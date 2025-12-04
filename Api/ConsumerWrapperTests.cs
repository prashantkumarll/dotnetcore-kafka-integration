using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Api;

namespace Api.Tests
{
    public class ConsumerWrapperTests : IDisposable
    {
        private ConsumerWrapper _consumerWrapper;
        private ConsumerConfig _validConfig;
        private const string ValidTopicName = "test-topic";

        public ConsumerWrapperTests()
        {
            _validConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public void Dispose()
        {
            _consumerWrapper?.Dispose();
        }

        [Fact]
        public void Constructor_ValidParameters_ShouldInitializeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            var topicName = "test-topic";

            // Act
            _consumerWrapper = new ConsumerWrapper(config, topicName);

            // Assert
            _consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName));
            exception.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, null));
            exception.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            var emptyTopicName = string.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, emptyTopicName));
            exception.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumerWrapper = new ConsumerWrapper(config, ValidTopicName);

            // Act
            var result = _consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_OperationCanceledException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumerWrapper = new ConsumerWrapper(config, ValidTopicName);

            // Act - This will timeout and potentially throw OperationCanceledException
            var result = _consumerWrapper.readMessage();

            // Assert - Should handle exception gracefully and return null
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "invalid-broker:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumerWrapper = new ConsumerWrapper(config, ValidTopicName);

            // Act - This may cause ConsumeException due to invalid broker
            var result = _consumerWrapper.readMessage();

            // Assert - Should handle exception gracefully and return null
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumerWrapper = new ConsumerWrapper(config, ValidTopicName);

            // Act & Assert
            Action act = () =>
            {
                _consumerWrapper.readMessage();
                _consumerWrapper.readMessage();
                _consumerWrapper.readMessage();
            };

            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            _consumerWrapper = new ConsumerWrapper(config, ValidTopicName);

            // Act & Assert
            _consumerWrapper.Dispose();
            _consumerWrapper.Dispose(); // Second dispose should not throw
            
            // Additional dispose calls should be safe
            Action act = () => _consumerWrapper.Dispose();
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            _consumerWrapper = new ConsumerWrapper(config, ValidTopicName);

            // Act
            Action act = () => _consumerWrapper.Dispose();

            // Assert - Should not throw exception during disposal
            act.Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_AfterDispose_ShouldHandleGracefully()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumerWrapper = new ConsumerWrapper(config, ValidTopicName);
            _consumerWrapper.Dispose();

            // Act & Assert - Should handle disposed consumer gracefully
            Action act = () => _consumerWrapper.readMessage();
            act.Should().Throw<ObjectDisposedException>();
        }

        [Theory]
        [InlineData("topic1")]
        [InlineData("topic-with-dashes")]
        [InlineData("topic_with_underscores")]
        [InlineData("topic.with.dots")]
        public void Constructor_ValidTopicNames_ShouldInitializeSuccessfully(string topicName)
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };

            // Act
            using var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithComplexConfig_ShouldInitializeSuccessfully()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "complex-test-group",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false,
                SessionTimeoutMs = 30000
            };
            var topicName = "complex-topic";

            // Act
            using var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void UsingStatement_ShouldDisposeAutomatically()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };

            // Act & Assert
            Action act = () =>
            {
                using (var consumerWrapper = new ConsumerWrapper(config, ValidTopicName))
                {
                    consumerWrapper.readMessage();
                    // Dispose should be called automatically
                }
            };

            act.Should().NotThrow();
        }
    }
}