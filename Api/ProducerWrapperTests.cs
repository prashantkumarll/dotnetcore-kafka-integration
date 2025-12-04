using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using System.Threading.Tasks;
using Api;

namespace Api.Tests
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly ProducerConfig _validConfig;
        private readonly string _validTopicName;

        public ProducerWrapperTests()
        {
            _validConfig = new ProducerConfig();
            _validTopicName = "test-topic";
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
            var exception = Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, topicName));
            exception.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(config, null));
            exception.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var message = "test-message";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                Func<Task> act = async () => await producerWrapper.writeMessage(message);

                // Assert - should not throw exception
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act & Assert
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
                exception.ParamName.Should().Be("message");
            }
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);
            Action act = () => producerWrapper.Dispose();

            // Assert - should not throw exception
            act.Should().NotThrow();
        }

        [Fact]
        public void MultipleDispose_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);
            Action act = () => {
                producerWrapper.Dispose();
                producerWrapper.Dispose(); // Second dispose should not throw
            };

            // Assert - should not throw exception
            act.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_LongMessage_ShouldProduceSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var longMessage = new string('x', 1000);

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                Func<Task> act = async () => await producerWrapper.writeMessage(longMessage);

                // Assert - should not throw exception
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task WriteMessage_EmptyString_ShouldProduceSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var emptyMessage = string.Empty;

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                Func<Task> act = async () => await producerWrapper.writeMessage(emptyMessage);

                // Assert - should not throw exception
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task WriteMessage_WhitespaceString_ShouldProduceSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var whitespaceMessage = "   ";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                Func<Task> act = async () => await producerWrapper.writeMessage(whitespaceMessage);

                // Assert - should not throw exception
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task WriteMessage_SpecialCharacters_ShouldProduceSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var specialMessage = "test@#$%^&*()message";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                Func<Task> act = async () => await producerWrapper.writeMessage(specialMessage);

                // Assert - should not throw exception
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task WriteMessage_UnicodeCharacters_ShouldProduceSuccessfully()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var unicodeMessage = "测试消息 🚀";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                Func<Task> act = async () => await producerWrapper.writeMessage(unicodeMessage);

                // Assert - should not throw exception
                await act.Should().NotThrowAsync();
            }
        }

        [Theory]
        [InlineData("simple")]
        [InlineData("message with spaces")]
        [InlineData("123456789")]
        [InlineData("mixed123content")]
        public async Task WriteMessage_VariousValidMessages_ShouldProduceSuccessfully(string message)
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                Func<Task> act = async () => await producerWrapper.writeMessage(message);

                // Assert - should not throw exception
                await act.Should().NotThrowAsync();
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        public async Task WriteMessage_VariousTopicNames_ShouldWork(string topicSuffix)
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = $"topic{topicSuffix}";
            var message = "test";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                Func<Task> act = async () => await producerWrapper.writeMessage(message);

                // Assert - should not throw exception
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig();
            var emptyTopicName = string.Empty;

            // Act
            Action act = () => {
                using var producerWrapper = new ProducerWrapper(config, emptyTopicName);
            };

            // Assert - empty topic name should be allowed
            act.Should().NotThrow();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}