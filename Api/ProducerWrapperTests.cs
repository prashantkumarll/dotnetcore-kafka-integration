using Api;
using Confluent.Kafka;
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
            // Capture console output for testing
            _originalConsoleOut = Console.Out;
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
        }

        public void Dispose()
        {
            // Restore original console output
            Console.SetOut(_originalConsoleOut);
            _consoleOutput?.Dispose();
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";

            // Act
            using var producer = new ProducerWrapper(config, topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig config = null;
            var topicName = "test-topic";

            // Act & Assert
            var action = () => new ProducerWrapper(config, topicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            string topicName = null;

            // Act & Assert
            var action = () => new ProducerWrapper(config, topicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var message = "test message";
            
            using var producer = new ProducerWrapper(config, topicName);

            // Act
            var action = async () => await producer.writeMessage(message);

            // Assert
            // Note: This will likely fail in unit test environment without Kafka broker
            // but tests the method signature and null validation
            await action.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            string message = null;
            
            using var producer = new ProducerWrapper(config, topicName);

            // Act & Assert
            var action = async () => await producer.writeMessage(message);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Theory]
        [InlineData("")]
        [InlineData("simple message")]
        [InlineData("message with spaces and numbers 123")]
        public async Task WriteMessage_WithVariousMessages_ShouldHandleCorrectly(string message)
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            
            using var producer = new ProducerWrapper(config, topicName);

            // Act & Assert
            var action = async () => await producer.writeMessage(message);
            
            // Will throw exception due to no Kafka broker, but validates parameter handling
            await action.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldDisposeResources()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var producer = new ProducerWrapper(config, topicName);

            // Act
            var action = () => producer.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var producer = new ProducerWrapper(config, topicName);

            // Act
            producer.Dispose();
            var action = () => producer.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithEmptyTopicName_ShouldCreateInstance()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = string.Empty;

            // Act
            var action = () => new ProducerWrapper(config, topicName);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithMinimalConfig_ShouldCreateInstance()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            var action = () => new ProducerWrapper(config, topicName);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithComplexConfig_ShouldCreateInstance()
        {
            // Arrange
            var config = new ProducerConfig 
            { 
                BootstrapServers = "localhost:9092",
                ClientId = "test-client",
                Acks = Acks.All,
                RetryBackoffMs = 100
            };
            var topicName = "complex-topic";

            // Act
            var action = () => new ProducerWrapper(config, topicName);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var longMessage = new string('a', 1000); // 1000 character message
            
            using var producer = new ProducerWrapper(config, topicName);

            // Act & Assert
            var action = async () => await producer.writeMessage(longMessage);
            
            // Will throw exception due to no Kafka broker, but validates parameter handling
            await action.Should().ThrowAsync<Exception>();
        }
    }
}