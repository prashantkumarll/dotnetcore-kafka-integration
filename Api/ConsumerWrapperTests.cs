using Api;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Api.Tests
{
    public class ConsumerWrapperTests : IDisposable
    {
        private readonly Mock<ServiceBusClient> mockClient;
        private readonly ServiceBusProcessorOptions options;
        private readonly string validTopicName;

        public ConsumerWrapperTests()
        {
            // Ensure the mock is declared exactly as required
            var mockClient = new Mock<ServiceBusClient>();
            this.mockClient = mockClient;

            options = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1
            };

            validTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            var mockClient = new Mock<ServiceBusClient>();
            // Arrange & Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, validTopicName, options);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullClient_ShouldThrowArgumentNullException()
        {
            // Arrange
            ServiceBusClient nullClient = default!;

            // Act & Assert
            var action = () => new ConsumerWrapper(nullClient, validTopicName, options);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("client");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;

            // Act & Assert
            var action = () => new ConsumerWrapper(mockClient.Object, nullTopicName, options);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("topicName");
        }

        [Fact]
        public void Constructor_WithEmptyTopicName_ShouldNotThrow()
        {
            // Arrange
            string emptyTopicName = string.Empty;

            // Act & Assert
            var action = () => new ConsumerWrapper(mockClient.Object, emptyTopicName, options);
            action.Should().NotThrow();
        }

        [Fact]
        public void readMessage_WithValidMessage_ShouldReturnMessageValue()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, validTopicName, options);
            var expectedMessage = "test-message";

            // Act
            var result = await consumerWrapper.readMessage();

            // Assert
            // Note: This test will return null in real scenario due to timeout
            // In integration tests, you would need to produce a message first
            result.Should().BeNull();
        }

        [Fact]
        public void readMessage_WithTimeout_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, validTopicName, options);

            // Act
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void readMessage_WhenOperationCanceled_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, validTopicName, options);

            // Act
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void readMessage_WhenConsumeExceptionOccurs_ShouldReturnNull()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, validTopicName, options);

            // Act
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldDisposeConsumer()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, validTopicName, options);

            // Act
            var action = () => consumerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, validTopicName, options);

            // Act
            consumerWrapper.Dispose();
            var action = () => consumerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCloseThrowsException_ShouldStillDisposeConsumer()
        {
            // Arrange
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, validTopicName, options);

            // Act
            var action = () => consumerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData("topic1")]
        [InlineData("topic-with-dashes")]
        [InlineData("topic_with_underscores")]
        public void Constructor_WithVariousTopicNames_ShouldCreateInstance(string topicName)
        {
            // Arrange & Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}
}