using Api;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests
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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithEmptyOrWhitespaceTopicName_ShouldThrowArgumentNullException(string topicName)
        {
            // Act & Assert
            var action = () => new ProducerWrapper(_validConfig, topicName);
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

        [Theory]
        [InlineData("simple message")]
        [InlineData("message with spaces")]
        [InlineData("123456789")]
        [InlineData("special chars !@#$%")]
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
        public async Task WriteMessage_WithEmptyString_ShouldCompleteSuccessfully()
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
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void UsingStatement_ShouldDisposeAutomatically()
        {
            // Arrange & Act
            var action = () =>
            {
                using var producer = new ProducerWrapper(_validConfig, _validTopicName);
                // Producer should be disposed automatically when leaving using block
            };

            // Assert
            action.Should().NotThrow();
        }

        public void Dispose()
        {
            // Cleanup test resources if needed
        }
    }
}