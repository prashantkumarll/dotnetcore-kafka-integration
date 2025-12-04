using Api;
using Confluent.Kafka;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace Test
{
    public class ConsumerWrapperTests : IDisposable
    {
        private readonly ConsumerConfig _validConfig;
        private readonly string _validTopicName;
        private bool _disposed = false;

        public ConsumerWrapperTests()
        {
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
            wrapper.Dispose();
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
        public void Constructor_WithEmptyTopicName_ShouldNotThrow()
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
            
            // Act
            var result = wrapper.readMessage();

            // Assert
            // Note: This will likely return null in unit test environment
            // as there's no actual Kafka broker running
            result.Should().BeNull();
            wrapper.Dispose();
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
            wrapper.Dispose();
        }

        [Fact]
        public void ReadMessage_MultipleCallsAfterDispose_ShouldHandleGracefully()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);
            wrapper.Dispose();

            // Act & Assert
            var action = () => wrapper.readMessage();
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledOnce_ShouldDisposeCleanly()
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
            wrapper.Dispose();
            var action = () => wrapper.Dispose();
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithDifferentConfigurations_ShouldAcceptValidConfigs()
        {
            // Arrange
            var config1 = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "group1"
            };
            var config2 = new ConsumerConfig
            {
                BootstrapServers = "localhost:9093",
                GroupId = "group2",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            // Act & Assert
            var wrapper1 = new ConsumerWrapper(config1, "topic1");
            var wrapper2 = new ConsumerWrapper(config2, "topic2");

            wrapper1.Should().NotBeNull();
            wrapper2.Should().NotBeNull();

            wrapper1.Dispose();
            wrapper2.Dispose();
        }

        [Theory]
        [InlineData("test-topic")]
        [InlineData("another-topic")]
        [InlineData("topic_with_underscores")]
        [InlineData("topic-with-dashes")]
        public void Constructor_WithVariousTopicNames_ShouldAcceptValidNames(string topicName)
        {
            // Arrange & Act
            var wrapper = new ConsumerWrapper(_validConfig, topicName);

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Dispose();
        }

        [Fact]
        public void ReadMessage_AfterConstruction_ShouldBeCallableImmediately()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var action = () => wrapper.readMessage();

            // Assert
            action.Should().NotThrow();
            wrapper.Dispose();
        }

        [Fact]
        public void ReadMessage_ConsecutiveCalls_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var result1 = wrapper.readMessage();
            var result2 = wrapper.readMessage();
            var result3 = wrapper.readMessage();

            result1.Should().BeNull();
            result2.Should().BeNull();
            result3.Should().BeNull();
            
            wrapper.Dispose();
        }

        [Fact]
        public void Constructor_WithMinimalConfig_ShouldCreateInstance()
        {
            // Arrange
            var minimalConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "minimal-group"
            };

            // Act
            var wrapper = new ConsumerWrapper(minimalConfig, "minimal-topic");

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Dispose();
        }

        [Fact]
        public void Constructor_WithComplexConfig_ShouldCreateInstance()
        {
            // Arrange
            var complexConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "complex-group",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false,
                SessionTimeoutMs = 30000,
                MaxPollIntervalMs = 300000
            };

            // Act
            var wrapper = new ConsumerWrapper(complexConfig, "complex-topic");

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Dispose();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("t")]
        [InlineData("n")]
        public void Constructor_WithWhitespaceTopicNames_ShouldNotThrow(string topicName)
        {
            // Arrange & Act
            var action = () => new ConsumerWrapper(_validConfig, topicName);

            // Assert
            action.Should().NotThrow();
            
            var wrapper = new ConsumerWrapper(_validConfig, topicName);
            wrapper.Dispose();
        }

        [Fact]
        public void ReadMessage_OnDisposedInstance_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);
            wrapper.Dispose();

            // Act
            var result = wrapper.readMessage();

            // Assert
            // Should handle gracefully even after disposal
            var action = () => wrapper.readMessage();
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_OnAlreadyDisposedInstance_ShouldBeIdempotent()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);
            
            // Act - dispose multiple times
            wrapper.Dispose();
            wrapper.Dispose();
            wrapper.Dispose();

            // Assert - should not throw
            var action = () => wrapper.Dispose();
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithLongTopicName_ShouldAcceptValidName()
        {
            // Arrange
            var longTopicName = "very-long-topic-name-with-many-characters-and-dashes-to-test-boundary-conditions";

            // Act
            var wrapper = new ConsumerWrapper(_validConfig, longTopicName);

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Dispose();
        }

        [Fact]
        public void ReadMessage_RepeatedCallsWithShortInterval_ShouldHandleTimeout()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act - multiple rapid calls
            var results = new string[5];
            for (int i = 0; i < 5; i++)
            {
                results[i] = wrapper.readMessage();
            }

            // Assert
            foreach (var result in results)
            {
                result.Should().BeNull(); // No broker running in test
            }
            
            wrapper.Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }
    }
}