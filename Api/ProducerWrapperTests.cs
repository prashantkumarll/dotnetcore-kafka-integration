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
        private readonly string _validTopicName;

        public ProducerWrapperTests()
        {
            // Arrange - Setup valid test configuration
            _validTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange & Act
            var mockClient = new Mock<ServiceBusClient>();
            using var producer = new ProducerWrapper(mockClient.Object, _validTopicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ServiceBusClient nullClient = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(nullClient, _validTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("client");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            var action = () => new ProducerWrapper(mockClient.Object, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithEmptyOrWhitespaceTopicName_ShouldThrowArgumentNullException(string topicName)
        {
            // Act & Assert
            var mockClient = new Mock<ServiceBusClient>();
            var action = () => new ProducerWrapper(mockClient.Object, topicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            using var producer = new ProducerWrapper(mockClient.Object, _validTopicName);
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
            var mockClient = new Mock<ServiceBusClient>();
            using var producer = new ProducerWrapper(mockClient.Object, _validTopicName);
            string nullMessage = default!;

            // Act & Assert
            var action = async () => await producer.writeMessage(nullMessage);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Theory]
        [InlineData("simple message")]
        [InlineData("message with numbers 123")]
        [InlineData("special chars !@#$%")]
        public async Task WriteMessage_WithVariousMessageFormats_ShouldHandleCorrectly(string message)
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            using var producer = new ProducerWrapper(mockClient.Object, _validTopicName);

            // Act
            var action = async () => await producer.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            using var producer = new ProducerWrapper(mockClient.Object, _validTopicName);
            var longMessage = new string('a', 10000);

            // Act
            var action = async () => await producer.writeMessage(longMessage);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldCompleteSuccessfully()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var producer = new ProducerWrapper(mockClient.Object, _validTopicName);

            // Act
            var action = () => producer.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var producer = new ProducerWrapper(mockClient.Object, _validTopicName);

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
            var mockClient = new Mock<ServiceBusClient>();
            var producer = new ProducerWrapper(mockClient.Object, _validTopicName);
            producer.Dispose();

            // Act & Assert
            var action = async () => await producer.writeMessage("test message");
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void UsingStatement_ShouldDisposeCorrectly()
        {
            // Arrange & Act
            var action = () =>
            {
                var mockClient = new Mock<ServiceBusClient>();
                using var producer = new ProducerWrapper(mockClient.Object, _validTopicName);
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
}