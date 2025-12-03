using Api;
using Confluent.Kafka;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests : IDisposable
    {
        private ProducerWrapper _producerWrapper;
        private readonly ProducerConfig _validConfig;
        private readonly string _validTopicName;

        public ProducerWrapperTests()
        {
            _validConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-client"
            };
            _validTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange & Act
            _producerWrapper = new ProducerWrapper(_validConfig, _validTopicName);

            // Assert
            _producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig nullConfig = null;

            // Act
            Action act = () => new ProducerWrapper(nullConfig, _validTopicName);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = null;

            // Act
            Action act = () => new ProducerWrapper(_validConfig, nullTopicName);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            _producerWrapper = new ProducerWrapper(_validConfig, _validTopicName);
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
            _producerWrapper = new ProducerWrapper(_validConfig, _validTopicName);
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
            _producerWrapper = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            Func<Task> act = async () => await _producerWrapper.writeMessage(message);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldNotThrow()
        {
            // Arrange
            _producerWrapper = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            Action act = () => _producerWrapper.Dispose();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            _producerWrapper = new ProducerWrapper(_validConfig, _validTopicName);

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
            _producerWrapper = new ProducerWrapper(_validConfig, _validTopicName);
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
            Action act = () => _producerWrapper = new ProducerWrapper(_validConfig, emptyTopicName);

            // Assert
            act.Should().NotThrow();
            _producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithMinimalConfig_ShouldCreateInstance()
        {
            // Arrange
            var minimalConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            // Act
            Action act = () => _producerWrapper = new ProducerWrapper(minimalConfig, _validTopicName);

            // Assert
            act.Should().NotThrow();
            _producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            _producerWrapper = new ProducerWrapper(_validConfig, _validTopicName);
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