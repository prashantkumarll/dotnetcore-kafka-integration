using Api;
using Confluent.Kafka;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly ProducerConfig _validConfig;
        private readonly string _validTopicName;

        public ProducerWrapperTests()
        {
            // Arrange - Setup valid test configuration
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
            // Arrange & Act
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig nullConfig = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(nullConfig, _validTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(_validConfig, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var testMessage = "test message";

            // Act & Assert
            var action = async () => await producer.writeMessage(testMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            string nullMessage = default!;

            // Act & Assert
            var action = async () => await producer.writeMessage(nullMessage);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public async Task WriteMessage_WithEmptyMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var emptyMessage = string.Empty;

            // Act & Assert
            var action = async () => await producer.writeMessage(emptyMessage);
            await action.Should().NotThrowAsync();
        }

        [Theory]
        [InlineData("simple message")]
        [InlineData("message with spaces")]
        [InlineData("123456789")]
        [InlineData("special chars !@#$%")]
        public async Task WriteMessage_WithVariousMessages_ShouldCompleteSuccessfully(string message)
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = async () => await producer.writeMessage(message);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalledOnce_ShouldCompleteSuccessfully()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = () => producer.Dispose();
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = () =>
            {
                producer.Dispose();
                producer.Dispose();
                producer.Dispose();
            };
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);
            producer.Dispose();

            // Act & Assert
            var action = async () => await producer.writeMessage("test message");
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void Constructor_WithDifferentTopicNames_ShouldCreateDistinctInstances()
        {
            // Arrange & Act
            using var producer1 = new ProducerWrapper(_validConfig, "topic1");
            using var producer2 = new ProducerWrapper(_validConfig, "topic2");

            // Assert
            producer1.Should().NotBeNull();
            producer2.Should().NotBeNull();
            producer1.Should().NotBeSameAs(producer2);
        }

        [Fact]
        public async Task WriteMessage_MultipleMessages_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var messages = new[] { "message1", "message2", "message3" };

            // Act & Assert
            foreach (var message in messages)
            {
                var action = async () => await producer.writeMessage(message);
                await action.Should().NotThrowAsync();
            }
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}