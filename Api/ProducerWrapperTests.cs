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
        private ProducerConfig _validConfig;
        private string _validTopicName;

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
            // Arrange
            var config = _validConfig;
            var topicName = _validTopicName;

            // Act
            var producer = new ProducerWrapper(config, topicName);

            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig config = null;
            var topicName = _validTopicName;

            // Act
            Action act = () => new ProducerWrapper(config, topicName);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = _validConfig;
            string topicName = null;

            // Act
            Action act = () => new ProducerWrapper(config, topicName);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_WithEmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = _validConfig;
            var topicName = string.Empty;

            // Act
            Action act = () => new ProducerWrapper(config, topicName);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldNotThrow()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var message = "test message";

            // Act
            Func<Task> act = async () => await producer.writeMessage(message);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            string message = null;

            // Act
            Func<Task> act = async () => await producer.writeMessage(message);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public async Task WriteMessage_WithEmptyMessage_ShouldNotThrow()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var message = string.Empty;

            // Act
            Func<Task> act = async () => await producer.writeMessage(message);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldNotThrow()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var message = new string('a', 1000);

            // Act
            Func<Task> act = async () => await producer.writeMessage(message);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithSpecialCharacters_ShouldNotThrow()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var message = "test with special chars: !@#$%^&*()";

            // Act
            Func<Task> act = async () => await producer.writeMessage(message);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            Action act = () => producer.Dispose();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            Action act = () =>
            {
                producer.Dispose();
                producer.Dispose();
                producer.Dispose();
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);
            producer.Dispose();
            var message = "test message";

            // Act
            Func<Task> act = async () => await producer.writeMessage(message);

            // Assert
            await act.Should().ThrowAsync<ObjectDisposedException>();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}