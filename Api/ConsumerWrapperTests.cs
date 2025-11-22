using System;
using Xunit;
using NUnit.Framework;
using Confluent.Kafka;
using Moq;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void Constructor_NullTopicName_ThrowsArgumentNullException()
        {
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(mockConfig, null));
        }

        [Fact]
        public void Constructor_NullConfig_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, "test-topic"));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ReturnsNull()
        {
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Returns((ConsumeResult<string, string>)null);

            using (var wrapper = new ConsumerWrapper(mockConfig, "test-topic"))
            {
                var result = wrapper.readMessage();
                Assert.Null(result);
            }
        }

        [Fact]
        public void Dispose_MultipleCalls_OnlyDisposesOnce()
        {
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var wrapper = new ConsumerWrapper(mockConfig, "test-topic");
            
            wrapper.Dispose();
            wrapper.Dispose(); // Should not throw
        }

        [Fact]
        public void ReadMessage_ConsumeException_HandledGracefully()
        {
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Throws(new ConsumeException(new Error()));

            using (var wrapper = new ConsumerWrapper(mockConfig, "test-topic"))
            {
                var result = wrapper.readMessage();
                Assert.Null(result);
            }
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ReturnsNull()
        {
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Throws(new OperationCanceledException());

            using (var wrapper = new ConsumerWrapper(mockConfig, "test-topic"))
            {
                var result = wrapper.readMessage();
                Assert.Null(result);
            }
        }
    }
}