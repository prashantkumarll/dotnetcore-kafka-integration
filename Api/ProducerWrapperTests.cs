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
        private readonly StringWriter _consoleOutput;
        private readonly TextWriter _originalConsoleOut;

        public ProducerWrapperTests()
        {
            _consoleOutput = new StringWriter();
            _originalConsoleOut = Console.Out;
            Console.SetOut(_consoleOutput);
        }

        public void Dispose()
        {
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
            string topicName = "test-topic";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, topicName));
            exception.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(config, null));
            exception.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = string.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(config, topicName));
            exception.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_WithComplexConfig_ShouldInitializeSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-client",
                Acks = Acks.All,
                RetryBackoffMs = 100,
                MessageTimeoutMs = 5000,
                RequestTimeoutMs = 30000
            };
            var topicName = "complex-test-topic";

            // Act
            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Assert
            producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessageSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var message = "test-message";

            using var producerWrapper = new ProducerWrapper(config, topicName);

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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
            exception.ParamName.Should().Be("message");
        }

        [Fact]
        public async Task WriteMessage_EmptyMessage_ShouldProduceSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var message = string.Empty;

            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            var action = async () => await producerWrapper.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_LongMessage_ShouldProduceSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var message = new string('a', 10000); // 10KB message

            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            var action = async () => await producerWrapper.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_SpecialCharacters_ShouldProduceSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var message = "Special chars: àáâãäåæçèéêë 中文 🚀 ntr";

            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            var action = async () => await producerWrapper.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_MultipleMessages_ShouldProduceAllSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var messages = new[] { "message1", "message2", "message3" };

            using var producerWrapper = new ProducerWrapper(config, topicName);

            // Act & Assert
            foreach (var message in messages)
            {
                var action = async () => await producerWrapper.writeMessage(message);
                await action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
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
            var message = "test-message";

            // Act
            producerWrapper.Dispose();

            // Assert
            var action = async () => await producerWrapper.writeMessage(message);
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void UsingStatement_ShouldDisposeAutomatically()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";

            // Act & Assert
            var action = () =>
            {
                using var producerWrapper = new ProducerWrapper(config, topicName);
                // Dispose should be called automatically
            };

            action.Should().NotThrow();
        }
    }
}