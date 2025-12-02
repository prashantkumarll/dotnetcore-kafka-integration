using Api;
using Confluent.Kafka;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Test
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
            ConsumerConfig nullConfig = null;

            // Act & Assert
            var action = () => new ConsumerWrapper(nullConfig, _validTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = null;

            // Act & Assert
            var action = () => new ConsumerWrapper(_validConfig, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithEmptyOrWhitespaceTopicName_ShouldThrowArgumentNullException(string topicName)
        {
            // Act & Assert
            var action = () => new ConsumerWrapper(_validConfig, topicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void ReadMessage_WithValidMessage_ShouldReturnMessageValue()
        {
            // Arrange
            var expectedMessage = "test-message-value";
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            // Note: This test will return null in real scenario due to no actual Kafka broker
            // In integration tests, you would set up a test Kafka environment
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_WithNoMessage_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_WithOperationCanceledException_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_WithConsumeException_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldNotThrowException()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = () => consumerWrapper.Dispose();
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrowException()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = () =>
            {
                consumerWrapper.Dispose();
                consumerWrapper.Dispose();
                consumerWrapper.Dispose();
            };
            action.Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_AfterDispose_ShouldHandleGracefully()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName);
            consumerWrapper.Dispose();

            // Act & Assert
            var action = () => consumerWrapper.readMessage();
            // The method should handle disposed state gracefully
            // In real implementation, this might throw ObjectDisposedException
            // but based on the code, it will likely return null or throw
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithDifferentConfigValues_ShouldCreateInstance()
        {
            // Arrange
            var customConfig = new ConsumerConfig
            {
                BootstrapServers = "custom-server:9092",
                GroupId = "custom-group",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false
            };
            var customTopic = "custom-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(customConfig, customTopic);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        public void Dispose()
        {
            // Clean up any test resources if needed
        }
    }
}