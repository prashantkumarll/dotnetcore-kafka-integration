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
        public async Task WriteMessage_WithValidMessage_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var message = "test message";

            // Act
            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                Func<Task> act = async () => await producerWrapper.writeMessage(message);

                // Assert
                await act.Should().NotThrowAsync();
            }
        }
    }
}