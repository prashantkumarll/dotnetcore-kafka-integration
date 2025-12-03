using Azure.Messaging.ServiceBus;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests : IDisposable
    {
        private ProducerWrapper _producerWrapper;
        private readonly string _validTopicName;

        public ProducerWrapperTests()
        {
            // ServiceBusClient will be mocked per test
            _validTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange & Act
            var mockClient = new Mock<ServiceBusClient>();
            _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);

            // Assert
            _producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ServiceBusClient nullClient = null;

            // Act
            Action act = () => new ProducerWrapper(nullClient, _validTopicName);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("client");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = null;

            // Act
            var mockClient = new Mock<ServiceBusClient>();
            Action act = () => new ProducerWrapper(mockClient.Object, nullTopicName);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);
            var testMessage = "test message";

            // Act
            Func<Task> act = async () => await _producerWrapper.writeMessage(testMessage);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);
            string nullMessage = null;

            // Act
            Func<Task> act = async () => await _producerWrapper.writeMessage(nullMessage);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Theory]
        [InlineData("")]
        [InlineData("simple message")]
        [InlineData("message with spaces and numbers 123")]
        public async Task WriteMessage_WithVariousValidMessages_ShouldCompleteSuccessfully(string message)
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);

            // Act
            Func<Task> act = async () => await _producerWrapper.writeMessage(message);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldNotThrow()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);

            // Act
            Action act = () => _producerWrapper.Dispose();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);

            // Act
            Action act = () =>
            {
                _producerWrapper.Dispose();
                _producerWrapper.Dispose();
                _producerWrapper.Dispose();
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);
            _producerWrapper.Dispose();

            // Act
            Func<Task> act = async () => await _producerWrapper.writeMessage("test message");

            // Assert
            await act.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void Constructor_WithEmptyTopicName_ShouldCreateInstance()
        {
            // Arrange
            var emptyTopicName = "";

            // Act
            var mockClient = new Mock<ServiceBusClient>();
            Action act = () => _producerWrapper = new ProducerWrapper(mockClient.Object, emptyTopicName);

            // Assert
            act.Should().NotThrow();
            _producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithMinimalConfig_ShouldCreateInstance()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            Action act = () => _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);

            // Assert
            act.Should().NotThrow();
            _producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            _producerWrapper = new ProducerWrapper(mockClient.Object, _validTopicName);
            var longMessage = new string('a', 1000);

            // Act
            Func<Task> act = async () => await _producerWrapper.writeMessage(longMessage);

            // Assert
            await act.Should().NotThrowAsync();
        }

        public void Dispose()
        {
            _producerWrapper?.Dispose();
        }
    }
}