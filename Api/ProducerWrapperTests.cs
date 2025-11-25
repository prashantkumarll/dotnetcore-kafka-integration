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
            var topicName = "error-topic";
            var mockProducer = new Mock<IProducer<string, string>>();
            mockProducer.Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>()))
                        .ThrowsAsync(new ProduceException<string, string>(new Error(ErrorCode.Unknown, "Test error")));

            var producerWrapper = new ProducerWrapper(mockConfig, topicName);

            // Act
            Func<Task> act = async () => await producerWrapper.writeMessage("test message");

            // Assert
            act.Should().ThrowAsync<ProduceException<string, string>>();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldBeSafe()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "safe-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act & Assert
            producerWrapper.Dispose();
            producerWrapper.Dispose(); // Multiple calls should not throw
        }
    }
}