using Api;
using Confluent.Kafka;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly ProducerConfig _validConfig;
        private readonly string _validTopicName;

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
            // Arrange & Act
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig nullConfig = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(nullConfig, _validTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(_validConfig, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var testMessage = "test message";

            // Act
            var action = async () => await producer.writeMessage(testMessage);

            // Assert - Should not throw exception (integration test)
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            string nullMessage = default!;

            // Act & Assert
            var action = async () => await producer.writeMessage(nullMessage);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public async Task WriteMessage_WithEmptyMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var emptyMessage = string.Empty;

            // Act
            var action = async () => await producer.writeMessage(emptyMessage);

            // Assert - Should not throw exception
            await action.Should().NotThrowAsync();
        }

        [Theory]
        [InlineData("simple message")]
        [InlineData("message with spaces")]
        [InlineData("123456789")]
        [InlineData("special chars !@#$%")]
        public async Task WriteMessage_WithVariousMessages_ShouldCompleteSuccessfully(string message)
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            var action = async () => await producer.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldCompleteSuccessfully()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act
            var action = () => producer.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

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
            var producer = new ProducerWrapper(_validConfig, _validTopicName);
            producer.Dispose();

            // Act & Assert
            var action = async () => await producer.writeMessage("test message");
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void Constructor_WithDifferentConfigurations_ShouldCreateInstances()
        {
            // Arrange
            var config1 = new ProducerConfig { BootstrapServers = "server1:9092" };
            var config2 = new ProducerConfig { BootstrapServers = "server2:9092", ClientId = "client2" };

            // Act & Assert
            using var producer1 = new ProducerWrapper(config1, "topic1");
            using var producer2 = new ProducerWrapper(config2, "topic2");

            producer1.Should().NotBeNull();
            producer2.Should().NotBeNull();
        }

        [Fact]
        public async Task WriteMessage_ConcurrentCalls_ShouldHandleMultipleMessages()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var tasks = new Task[5];

            // Act
            for (int i = 0; i < 5; i++)
            {
                int messageIndex = i;
                tasks[i] = producer.writeMessage($"concurrent message {messageIndex}");
            }

            // Assert
            var action = async () => await Task.WhenAll(tasks);
            await action.Should().NotThrowAsync();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}