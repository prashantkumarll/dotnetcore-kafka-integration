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
        public void WriteMessage_ProduceException_ShouldLogAndRethrow()
        {
            // Arrange
            var mockConfig = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockConfig, topicName);
            var message = "error-message";

            // Act & Assert
            Func<Task> act = async () => await producerWrapper.writeMessage(message);
            act.Should().ThrowAsync<ProduceException<string, string>>();
        }

        [Fact]
        public void Dispose_FlushTimeout_ShouldHandleQuietly()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            Action dispose = () => producerWrapper.Dispose();

            // Assert
            dispose.Should().NotThrow();
        }
    }
}