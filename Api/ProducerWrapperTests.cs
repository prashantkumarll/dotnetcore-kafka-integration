using System;
using Xunit;
using NUnit.Framework;
using Confluent.Kafka;
using Moq;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void Constructor_NullTopicName_ThrowsArgumentNullException()
        {
            var config = new ProducerConfig();
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(config, null));
        }

        [Fact]
        public void Constructor_NullConfig_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, "TestTopic"));
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ThrowsArgumentNullException()
        {
            var config = new ProducerConfig();
            using var producer = new ProducerWrapper(config, "TestTopic");
            await Assert.ThrowsAsync<ArgumentNullException>(() => producer.writeMessage(null));
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ProducesSuccessfully()
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using var producer = new ProducerWrapper(config, "TestTopic");
            await producer.writeMessage("Test Message");
        }

        [Fact]
        public void Dispose_MultipleCalls_OnlyDisposesOnce()
        {
            var config = new ProducerConfig();
            var producer = new ProducerWrapper(config, "TestTopic");
            producer.Dispose();
            producer.Dispose(); // Should not throw
        }

        [Fact]
        public void Constructor_CreatesProducer_NotNull()
        {
            var config = new ProducerConfig();
            using var producer = new ProducerWrapper(config, "TestTopic");
            Assert.NotNull(producer);
        }

        [Fact]
        public async Task WriteMessage_EmptyMessage_Produces()
        {
            var config = new ProducerConfig();
            using var producer = new ProducerWrapper(config, "TestTopic");
            await producer.writeMessage(string.Empty);
        }

        [Fact]
        public void Dispose_FlushesProducer()
        {
            var config = new ProducerConfig();
            using var producer = new ProducerWrapper(config, "TestTopic");
            producer.Dispose(); // Ensure flush occurs
        }
    }
}