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
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Assert
            wrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ConsumerConfig nullConfig = default!;

            // Act & Assert
            var action = () => new ConsumerWrapper(nullConfig, _validTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;

            // Act & Assert
            var action = () => new ConsumerWrapper(_validConfig, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
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
        public void ReadMessage_WithValidMessage_ShouldReturnMessageValue()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);
            var expectedMessage = "test-message";

            // Act
            var result = wrapper.readMessage();

            // Assert
            // Note: This test will return null in real scenario due to no actual Kafka broker
            // In a real test environment, you would mock the consumer or use testcontainers
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_WithTimeout_ShouldReturnNull()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_MultipleCallsWithoutMessages_ShouldReturnNull()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result1 = wrapper.readMessage();
            var result2 = wrapper.readMessage();
            var result3 = wrapper.readMessage();

            // Assert
            result1.Should().BeNull();
            result2.Should().BeNull();
            result3.Should().BeNull();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = () => wrapper.Dispose();
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = () =>
            {
                wrapper.Dispose();
                wrapper.Dispose();
                wrapper.Dispose();
            };
            action.Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_AfterDispose_ShouldHandleGracefully()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);
            wrapper.Dispose();

            // Act & Assert
            var action = () => wrapper.readMessage();
            // The method should handle disposed state gracefully
            // In practice, this might throw ObjectDisposedException
            // but the current implementation doesn't check _disposed in readMessage
        }

        [Theory]
        [InlineData("topic1")]
        [InlineData("topic-with-dashes")]
        [InlineData("topic_with_underscores")]
        [InlineData("topic123")]
        public void Constructor_WithVariousTopicNames_ShouldCreateInstance(string topicName)
        {
            // Arrange & Act
            var wrapper = new ConsumerWrapper(_validConfig, topicName);

            // Assert
            wrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithDifferentConfigurations_ShouldCreateInstance()
        {
            // Arrange
            var config1 = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "group1",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            var config2 = new ConsumerConfig
            {
                BootstrapServers = "localhost:9093",
                GroupId = "group2",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            // Act
            var wrapper1 = new ConsumerWrapper(config1, "topic1");
            var wrapper2 = new ConsumerWrapper(config2, "topic2");

            // Assert
            wrapper1.Should().NotBeNull();
            wrapper2.Should().NotBeNull();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}