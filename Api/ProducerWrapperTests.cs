{
  "testCasesFound": 0,
  "newTestCasesAdded": 8,
  "generatedTestCode": "using System;
using Xunit;
using NUnit.Framework;
using Moq;
using Confluent.Kafka;
using Api;
using System.Threading.Tasks;

namespace Api.Tests
{
    [TestClass]
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
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, \"TestTopic\"));
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ThrowsArgumentNullException()
        {
            var config = new ProducerConfig();
            using (var producer = new ProducerWrapper(config, \"TestTopic\"))
            {
               ...