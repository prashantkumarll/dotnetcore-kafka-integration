using System;
using Xunit;
using FluentAssertions;
using Confluent.Kafka;
using Moq;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Assert
            producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, topicName));
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(config, null));
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var message = "test message";

            // Act
            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                var act = async () => await producerWrapper.writeMessage(message);

                // Assert
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act & Assert
            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                var act = async () => await producerWrapper.writeMessage(null);
                await act.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task WriteMessage_WithEmptyMessage_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var message = string.Empty;

            // Act
            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                var act = async () => await producerWrapper.writeMessage(message);

                // Assert
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void Dispose_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            var act = () => producerWrapper.Dispose();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            var act = () => {
                producerWrapper.Dispose();
                producerWrapper.Dispose();
            };

            // Assert
            act.Should().NotThrow();
        }
    }
}