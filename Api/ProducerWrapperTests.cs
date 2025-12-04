using Api;
using Confluent.Kafka;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
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
            // Arrange & Act
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Assert
            producer.Should().NotBeNull();
            producer.Dispose();
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

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var longMessage = new string('a', 1000);

            // Act & Assert
            var action = async () => await producer.writeMessage(longMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithSpecialCharacters_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var specialMessage = "test@#$%^&*()_+{}|:<>?[]\;'",.";

            // Act & Assert
            var action = async () => await producer.writeMessage(specialMessage);
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
        public void UsingStatement_ShouldDisposeAutomatically()
        {
            // Arrange & Act & Assert
            var action = () =>
            {
                using var producer = new ProducerWrapper(_validConfig, _validTopicName);
                // Producer should be disposed automatically when leaving using block
            };
            action.Should().NotThrow();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}