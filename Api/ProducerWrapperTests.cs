using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void Constructor_ValidConfig_ShouldInitializeProducer()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "testtopic";

            // Act
            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Assert
                producerWrapper.Should().NotBeNull();
            }
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ProducerWrapper(null, "topic");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*config*");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act
            Action act = () => new ProducerWrapper(config, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*topicName*");
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "testtopic";
            var message = "testmessage";

            // Act
            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                Func<Task> act = async () => await producerWrapper.writeMessage(message);

                // Assert
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "testtopic";

            // Act
            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                Func<Task> act = async () => await producerWrapper.writeMessage(null);

                // Assert
                await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("*message*");
            }
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "testtopic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);
            producerWrapper.Dispose();

            // Assert
            producerWrapper.Dispose(); // Second dispose should not throw
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "testtopic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);
            producerWrapper.Dispose();
            producerWrapper.Dispose(); // Multiple dispose calls

            // Assert
            // No exception should be thrown
        }
    }
}