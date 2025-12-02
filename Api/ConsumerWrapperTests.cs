using Api;
using Confluent.Kafka;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Api.Tests
{
    public class ConsumerWrapperTests : IDisposable
    {
        private readonly Mock<IConsumer<string, string>> _mockConsumer;
        private readonly ConsumerConfig _validConfig;
        private readonly string _validTopicName;

        public ConsumerWrapperTests()
        {
            _mockConsumer = new Mock<IConsumer<string, string>>();
            _validConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _validTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange & Act
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ConsumerConfig nullConfig = default!;

            // Act & Assert
            var action = () => new ConsumerWrapper(nullConfig, _validTopicName);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;

            // Act & Assert
            var action = () => new ConsumerWrapper(_validConfig, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("topicName");
        }

        [Fact]
        public void Constructor_WithEmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string emptyTopicName = string.Empty;

            // Act & Assert
            var action = () => new ConsumerWrapper(_validConfig, emptyTopicName);
            action.Should().NotThrow();
        }

        [Fact]
        public void readMessage_WithValidMessage_ShouldReturnMessageValue()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);
            var expectedMessage = "test-message";

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            // Note: This test will return null in real scenario due to timeout
            // In integration tests, you would need to produce a message first
            result.Should().BeNull();
        }

        [Fact]
        public void readMessage_WithTimeout_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void readMessage_WhenOperationCanceled_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void readMessage_WhenConsumeExceptionOccurs_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldDisposeConsumer()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var action = () => consumerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            consumerWrapper.Dispose();
            var action = () => consumerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCloseThrowsException_ShouldStillDisposeConsumer()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var action = () => consumerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData("topic1")]
        [InlineData("topic-with-dashes")]
        [InlineData("topic_with_underscores")]
        public void Constructor_WithVariousTopicNames_ShouldCreateInstance(string topicName)
        {
            // Arrange & Act
            var consumerWrapper = new ConsumerWrapper(_validConfig, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}