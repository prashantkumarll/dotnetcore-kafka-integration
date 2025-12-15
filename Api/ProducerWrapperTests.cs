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
        private readonly Mock<IProducer<string, string>> _mockProducer;
        private readonly ProducerConfig _testConfig;
        private readonly string _testTopicName;

        public ProducerWrapperTests()
        {
            _mockProducer = new Mock<IProducer<string, string>>();
            _testConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-producer"
            };
            _testTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange & Act
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);

            // Assert
            wrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig nullConfig = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(nullConfig, _testTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(_testConfig, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);
            var testMessage = "test message";

            // Act
            var action = async () => await wrapper.writeMessage(testMessage);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);
            string nullMessage = default!;

            // Act & Assert
            var action = async () => await wrapper.writeMessage(nullMessage);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public async Task WriteMessage_WithEmptyMessage_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);
            var emptyMessage = string.Empty;

            // Act
            var action = async () => await wrapper.writeMessage(emptyMessage);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);
            var longMessage = new string('a', 10000);

            // Act
            var action = async () => await wrapper.writeMessage(longMessage);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Dispose_WhenCalled_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);

            // Act
            var action = () => wrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);

            // Act
            wrapper.Dispose();
            var action = () => wrapper.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_AfterUsingStatement_ShouldDisposeCorrectly()
        {
            // Arrange & Act
            var action = () =>
            {
                using (var wrapper = new ProducerWrapper(_testConfig, _testTopicName))
                {
                    // Use wrapper within using block
                }
            };

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData("simple message")]
        [InlineData("message with spaces")]
        [InlineData("123456789")]
        [InlineData("special chars !@#$%")]
        public async Task WriteMessage_WithVariousMessages_ShouldNotThrow(string message)
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);

            // Act
            var action = async () => await wrapper.writeMessage(message);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_testConfig, _testTopicName);
            wrapper.Dispose();

            // Act & Assert
            var action = async () => await wrapper.writeMessage("test message");
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        public void Dispose()
        {
            // Cleanup test resources if needed
        }
    }
}