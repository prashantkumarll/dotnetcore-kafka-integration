using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Confluent.Kafka;
using Api;

namespace Api.Tests
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly ProducerConfig _validConfig;
        private readonly string _validTopicName;

        public ProducerWrapperTests()
        {
            _validConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-producer"
            };
            _validTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Act
            var wrapper = new ProducerWrapper(_validConfig, _validTopicName);

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Dispose();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ProducerWrapper(null, _validTopicName);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ProducerWrapper(_validConfig, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            using var wrapper = new ProducerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = async () => await wrapper.writeMessage(null);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldNotThrow()
        {
            // Arrange
            using var wrapper = new ProducerWrapper(_validConfig, _validTopicName);
            var testMessage = "test message";

            // Act & Assert
            var action = async () => await wrapper.writeMessage(testMessage);
            await action.Should().NotThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_validConfig, _validTopicName);

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