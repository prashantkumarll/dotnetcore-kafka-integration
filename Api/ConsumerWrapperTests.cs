using System;
using Xunit;
using FluentAssertions;
using Confluent.Kafka;
using Api;

namespace Api.Tests
{
    public class ConsumerWrapperTests : IDisposable
    {
        private ConsumerConfig _validConfig;
        private string _validTopicName;

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
            // Act & Assert
            var action = () => new ConsumerWrapper(_validConfig, _validTopicName);
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ConsumerWrapper(null, _validTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ConsumerWrapper(_validConfig, null);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void ReadMessage_WhenNoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            using var wrapper = new ConsumerWrapper(_validConfig, _validTopicName);

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
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

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}