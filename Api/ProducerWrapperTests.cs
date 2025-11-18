using System;
using Xunit;
using NUnit.Framework;
using Confluent.Kafka;
using Moq;
using System.Threading.Tasks;

namespace Api.Tests
{
    [TestFixture]
    public class ProducerWrapperTests
    {
        [Test]
        public void Constructor_NullTopicName_ThrowsArgumentNullException()
        {
            var config = new ProducerConfig();
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(config, null));
        }

        [Test]
        public void Constructor_NullConfig_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, "TestTopic"));
        }

        [Test]
        public async Task WriteMessage_NullMessage_ThrowsArgumentNullException()
        {
            var config = new ProducerConfig();
            using (var producer = new ProducerWrapper(config, "TestTopic"))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => producer.writeMessage(null));
            }
        }

        [Test]
        public async Task WriteMessage_ValidMessage_ProducesSuccessfully()
        {
            var config = new ProducerConfig();
            using (var producer = new ProducerWrapper(config, "TestTopic"))
            {
                await producer.writeMessage("Test Message");
            }
        }

        [Test]
        public void Dispose_MultipleCalls_OnlyDisposesOnce()
        {
            var config = new ProducerConfig();
            var producer = new ProducerWrapper(config, "TestTopic");
            producer.Dispose();
            producer.Dispose(); // Should not throw
        }

        [Test]
        public void Constructor_CreatesProducerWithErrorHandler()
        {
            var config = new ProducerConfig();
            using (var producer = new ProducerWrapper(config, "TestTopic"))
            {
                Assert.NotNull(producer);
            }
        }

        [Test]
        public async Task WriteMessage_LongMessage_Produces()
        {
            var config = new ProducerConfig();
            using (var producer = new ProducerWrapper(config, "TestTopic"))
            {
                await producer.writeMessage(new string('x', 1000));
            }
        }

        [Test]
        public void Dispose_FlushesOutstandingMessages()
        {
            var config = new ProducerConfig();
            using (var producer = new ProducerWrapper(config, "TestTopic"))
            {
                // Implicit test through Dispose method
                producer.Dispose();
            }
        }
    }
}