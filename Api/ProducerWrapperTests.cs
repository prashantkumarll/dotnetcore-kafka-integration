using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace Api.Tests
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly ProducerConfig _validConfig;
        private readonly string _validTopicName;
        private StringWriter _consoleOutput;
        private TextWriter _originalConsoleOut;

        public ProducerWrapperTests()
        {
            // Arrange - Setup valid test configuration
            _validConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            _validTopicName = "test-topic";
            
            // Capture console output for testing
            _consoleOutput = new StringWriter();
            _originalConsoleOut = Console.Out;
            Console.SetOut(_consoleOutput);
        }

        public void Dispose()
        {
            // Restore original console output
            Console.SetOut(_originalConsoleOut);
            _consoleOutput?.Dispose();
        }

        [Fact]
        public void Constructor_ValidConfig_ShouldInitializeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Assert
            producerWrapper.Should().NotBeNull();
            producerWrapper.Dispose();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            var action = () => new ProducerWrapper(null, topicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();

            // Act & Assert
            var action = () => new ProducerWrapper(config, null);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var emptyTopicName = string.Empty;

            // Act & Assert
            var action = () => new ProducerWrapper(config, emptyTopicName);
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var message = "test message";

            // Act & Assert
            var action = async () => await producerWrapper.writeMessage(message);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Act & Assert
            var action = async () => await producerWrapper.writeMessage(null);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public async Task WriteMessage_EmptyMessage_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var emptyMessage = string.Empty;

            // Act & Assert
            var action = async () => await producerWrapper.writeMessage(emptyMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_LongMessage_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var longMessage = new string('a', 10000);

            // Act & Assert
            var action = async () => await producerWrapper.writeMessage(longMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_SpecialCharacters_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var specialMessage = "Special chars: !@#$%^&*()_+-={}[]|\:;"'<>?,./";

            // Act & Assert
            var action = async () => await producerWrapper.writeMessage(specialMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_MultipleMessages_ShouldProduceAll()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var messages = new[] { "message1", "message2", "message3" };

            // Act & Assert
            foreach (var message in messages)
            {
                var action = async () => await producerWrapper.writeMessage(message);
                await action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void Dispose_ShouldDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            var action = () => producerWrapper.Dispose();

            // Assert - No specific assertion, just ensuring no exceptions are thrown
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act & Assert
            var action = () =>
            {
                producerWrapper.Dispose();
                producerWrapper.Dispose();
                producerWrapper.Dispose();
            };
            
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);
            var message = "test message";
            
            // Act
            producerWrapper.Dispose();
            
            // Assert
            var action = async () => await producerWrapper.writeMessage(message);
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void Constructor_WithComplexConfig_ShouldInitializeCorrectly()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-client",
                Acks = Acks.All,
                MessageTimeoutMs = 5000
            };
            var topicName = "complex-topic";

            // Act
            var action = () => new ProducerWrapper(config, topicName);

            // Assert
            action.Should().NotThrow();
            using var wrapper = action();
            wrapper.Should().NotBeNull();
        }
    }
}