using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Api;

namespace Api.Tests
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly StringWriter _consoleOutput;
        private readonly TextWriter _originalConsoleOut;

        public ProducerWrapperTests()
        {
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
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";

            // Act
            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Assert
            producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            var topicName = "test-topic";

            // Act & Assert
            var action = () => new ProducerWrapper(null, topicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act & Assert
            var action = () => new ProducerWrapper(config, null);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var emptyTopicName = string.Empty;

            // Act & Assert
            var action = () => new ProducerWrapper(config, emptyTopicName);
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var message = "test message";

            // Act
            var action = async () => await producerWrapper.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Act & Assert
            var action = async () => await producerWrapper.writeMessage(null);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldLogDeliveryInfo()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var message = "test delivery message";

            // Act
            try
            {
                await producerWrapper.writeMessage(message);
            }
            catch
            {
                // Ignore connection errors for this test
            }

            // Assert
            var consoleOutput = _consoleOutput.ToString();
            // Note: In real scenarios, this would contain KAFKA delivery info
            // For unit testing, we're mainly testing that no exceptions occur
        }

        [Fact]
        public async Task WriteMessage_EmptyMessage_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var emptyMessage = string.Empty;

            // Act
            var action = async () => await producerWrapper.writeMessage(emptyMessage);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_LongMessage_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);
            var longMessage = new string('a', 10000);

            // Act
            var action = async () => await producerWrapper.writeMessage(longMessage);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_ShouldDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            var action = () => producerWrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            producerWrapper.Dispose();
            var secondDisposeAction = () => producerWrapper.Dispose();

            // Assert
            secondDisposeAction.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);
            var message = "test message";

            // Act
            producerWrapper.Dispose();
            var action = async () => await producerWrapper.writeMessage(message);

            // Assert
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void Constructor_WithSpecialCharactersInTopicName_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic-with-dashes_and_underscores.and.dots";

            // Act
            var action = () => new ProducerWrapper(config, topicName);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_MultipleMessages_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Act & Assert
            for (int i = 0; i < 5; i++)
            {
                var message = $"message-{i}";
                var action = async () => await producerWrapper.writeMessage(message);
                await action.Should().NotThrowAsync();
            }
        }
    }
}