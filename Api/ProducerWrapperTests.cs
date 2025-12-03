using Api;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly StringWriter _consoleOutput;
        private readonly TextWriter _originalConsoleOut;

        public ProducerWrapperTests()
        {
            // Arrange - Capture console output for testing
            _originalConsoleOut = Console.Out;
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
        }

        public void Dispose()
        {
            // Cleanup - Restore original console output
            Console.SetOut(_originalConsoleOut);
            _consoleOutput?.Dispose();
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";

            // Act
            using var producer = new ProducerWrapper(mockClient.Object, topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ServiceBusClient client = default!;
            var topicName = "test-topic";

            // Act & Assert
            var action = () => new ProducerWrapper(client, topicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("client");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            string topicName = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(mockClient.Object, topicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var message = "test message";
            using var producer = new ProducerWrapper(mockClient.Object, topicName);

            // Act
            var action = async () => await producer.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            string message = default!;
            using var producer = new ProducerWrapper(mockClient.Object, topicName);

            // Act & Assert
            var action = async () => await producer.writeMessage(message);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public async Task WriteMessage_WithEmptyMessage_ShouldProduceMessage()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var message = string.Empty;
            using var producer = new ProducerWrapper(mockClient.Object, topicName);

            // Act
            var action = async () => await producer.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldProduceMessage()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var message = new string('a', 1000);
            using var producer = new ProducerWrapper(mockClient.Object, topicName);

            // Act
            var action = async () => await producer.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithSpecialCharacters_ShouldProduceMessage()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var message = "Special chars: !@#$%^&*()_+-={}[]|\\:;\"'<>?,./";
            using var producer = new ProducerWrapper(mockClient.Object, topicName);

            // Act
            var action = async () => await producer.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldNotThrow()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producer = new ProducerWrapper(mockClient.Object, topicName);

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
            var topicName = "test-topic";
            var producer = new ProducerWrapper(mockClient.Object, topicName);

            // Act
            var action = () =>
            {
                producer.Dispose();
                producer.Dispose();
                producer.Dispose();
            };

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void UsingStatement_ShouldDisposeCorrectly()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";

            // Act & Assert
            var action = () =>
            {
                using var producer = new ProducerWrapper(mockClient.Object, topicName);
                // Producer should be disposed automatically
            };

            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producer = new ProducerWrapper(mockClient.Object, topicName);
            var message = "test message";

            // Act
            producer.Dispose();
            var action = async () => await producer.writeMessage(message);

            // Assert
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }
    }
}
}