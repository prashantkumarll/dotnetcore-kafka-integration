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
        private readonly ProducerConfig _testConfig;
        private readonly string _testTopicName;

        public ProducerWrapperTests()
        {
            _testConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-producer"
            };
            _testTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Act
            using var wrapper = new ProducerWrapper(_testConfig, _testTopicName);

            // Assert
            wrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var act = () => new ProducerWrapper(null, _testTopicName);
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var act = () => new ProducerWrapper(_testConfig, null);
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            using var wrapper = new ProducerWrapper(_testConfig, _testTopicName);

            // Act & Assert
            var act = async () => await wrapper.writeMessage(null);
            await act.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);

            // Act & Assert
            var act = () =>
            {
                wrapper.Dispose();
                wrapper.Dispose();
            };
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_ShouldDisposeResources()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);

            // Act & Assert
            var act = () => wrapper.Dispose();
            act.Should().NotThrow();
        }

        public void Dispose()
        {
            // Cleanup test resources if needed
        }
    }
}