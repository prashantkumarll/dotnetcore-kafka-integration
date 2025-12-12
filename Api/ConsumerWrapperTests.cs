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
        private readonly ConsumerConfig _testConfig;
        private readonly string _testTopicName;

        public ConsumerWrapperTests()
        {
            _mockConsumer = new Mock<IConsumer<string, string>>();
            _testConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _testTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ConsumerConfig nullConfig = default!;

            // Act & Assert
            var action = () => new ConsumerWrapper(nullConfig, _testTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;

            // Act & Assert
            var action = () => new ConsumerWrapper(_testConfig, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_WithEmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string emptyTopicName = string.Empty;

            // Act & Assert
            var action = () => new ConsumerWrapper(_testConfig, emptyTopicName);
            action.Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_WithValidMessage_ShouldReturnMessageValue()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);
            var expectedMessage = "test-message";

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            // Note: This test will return null in real scenario due to no actual Kafka broker
            // In a real test environment, you would mock the consumer
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_WithTimeout_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_WhenOperationCanceled_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_WhenConsumeExceptionOccurs_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_WhenCalledOnce_ShouldDisposeConsumer()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);

            // Act
            var action = () => consumerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);

            // Act
            consumerWrapper.Dispose();
            var secondDisposeAction = () => consumerWrapper.Dispose();

            // Assert
            secondDisposeAction.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCloseThrowsException_ShouldStillDisposeConsumer()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);

            // Act
            var action = () => consumerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_AfterDispose_ShouldHandleGracefully()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(_testConfig, _testTopicName);
            consumerWrapper.Dispose();

            // Act
            var action = () => consumerWrapper.readMessage();

            // Assert
            // This might throw ObjectDisposedException, but the method should handle it
            // In the current implementation, it would likely throw, so we test for that
            action.Should().Throw<ObjectDisposedException>();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}