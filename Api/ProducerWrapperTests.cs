using Api;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly ServiceBusClient _validConfig;
        private readonly string _validTopicName;

        public ProducerWrapperTests()
        {
            // Arrange - Setup valid test configuration
            _validConfig = new ServiceBusClient
            {
                ConnectionString = "localhost:9092",
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
            ServiceBusClient nullConfig = default!;

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

            // Act
            var action = async () => await producer.writeMessage(testMessage);

            // Assert - Should not throw exception (integration test)
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
            await action.Should().Throw<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Theory]
        [InlineData("")]
        [InlineData("simple message")]
        [InlineData("message with spaces and numbers 123")]
        public async Task WriteMessage_WithVariousValidMessages_ShouldCompleteSuccessfully(string message)
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            var action = async () => await producer.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldCompleteSuccessfully()
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
            producer.Dispose();
            var secondDisposeAction = () => producer.Dispose();

            // Assert
            secondDisposeAction.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);
            producer.Dispose();

            // Act & Assert
            var action = async () => await producer.writeMessage("test message");
            await action.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void Constructor_WithEmptyTopicName_ShouldCreateInstance()
        {
            // Arrange
            var emptyTopicName = string.Empty;

            // Act
            var action = () => new ProducerWrapper(_validConfig, emptyTopicName);

            // Assert
            action.Should().NotThrow();
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
        public void UsingStatement_ShouldDisposeCorrectly()
        {
            // Arrange & Act
            var action = () =>
            {
                using var producer = new ProducerWrapper(_validConfig, _validTopicName);
                // Producer should be disposed automatically
            };

            // Assert
            action.Should().NotThrow();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}