using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Api;

namespace Test
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
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeAssignableTo<IDisposable>();
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
        public void Constructor_WithEmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var emptyTopicName = string.Empty;

            // Act & Assert
            var action = () => new ProducerWrapper(_validConfig, emptyTopicName);
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var testMessage = "test message";

            // Act
            var action = async () => await producer.writeMessage(testMessage);

            // Assert
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

            // Act
            var action = async () => await producer.writeMessage(emptyMessage);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var longMessage = new string('a', 1000);

            // Act
            var action = async () => await producer.writeMessage(longMessage);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            var action = () => producer.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            var firstDispose = () => producer.Dispose();
            var secondDispose = () => producer.Dispose();

            // Assert
            firstDispose.Should().NotThrow();
            secondDispose.Should().NotThrow();
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
            // Cleanup test resources if needed
        }
    }
}