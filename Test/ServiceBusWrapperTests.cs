using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Api;
using System;

namespace Test
{
    public class ServiceBusWrapperTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<ProducerWrapper>> _mockProducerLogger;
        private readonly Mock<ILogger<ConsumerWrapper>> _mockConsumerLogger;

        public ServiceBusWrapperTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockProducerLogger = new Mock<ILogger<ProducerWrapper>>();
            _mockConsumerLogger = new Mock<ILogger<ConsumerWrapper>>();

            SetupDefaultConfiguration();
        }

        private void SetupDefaultConfiguration()
        {
            _mockConfiguration
                .Setup(x => x["ServiceBus:ConnectionString"])
                .Returns("Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=test;SharedAccessKey=test");
            _mockConfiguration
                .Setup(x => x["ServiceBus:QueueName"])
                .Returns("orders");
        }

        [Fact]
        public void ProducerWrapper_Constructor_ValidConfiguration_Success()
        {
            // Act
            var producer = new ProducerWrapper(_mockConfiguration.Object, _mockProducerLogger.Object);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Constructor_MissingConnectionString_ThrowsException()
        {
            // Arrange
            _mockConfiguration
                .Setup(x => x["ServiceBus:ConnectionString"])
                .Returns((string?)null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new ProducerWrapper(_mockConfiguration.Object, _mockProducerLogger.Object));
        }

        [Fact]
        public void ProducerWrapper_Constructor_MissingQueueName_ThrowsException()
        {
            // Arrange
            _mockConfiguration
                .Setup(x => x["ServiceBus:QueueName"])
                .Returns((string?)null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new ProducerWrapper(_mockConfiguration.Object, _mockProducerLogger.Object));
        }

        [Fact]
        public async Task ProducerWrapper_ProduceAsync_ValidMessage_Success()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConfiguration.Object, _mockProducerLogger.Object);
            var testMessage = "Test message";

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => producer.ProduceAsync(testMessage));
            exception.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ProducerWrapper_ProduceAsync_InvalidMessage_ThrowsException(string message)
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConfiguration.Object, _mockProducerLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => producer.ProduceAsync(message));
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ValidConfiguration_Success()
        {
            // Act
            var consumer = new ConsumerWrapper(_mockConfiguration.Object, _mockConsumerLogger.Object);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_MissingConnectionString_ThrowsException()
        {
            // Arrange
            _mockConfiguration
                .Setup(x => x["ServiceBus:ConnectionString"])
                .Returns((string?)null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new ConsumerWrapper(_mockConfiguration.Object, _mockConsumerLogger.Object));
        }

        [Fact]
        public async Task ConsumerWrapper_StartConsumingAsync_Success()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConfiguration.Object, _mockConsumerLogger.Object);

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => consumer.StartConsumingAsync());
            exception.Should().BeNull();
        }

        [Fact]
        public async Task ProducerWrapper_MultipleMessages_AllSent()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConfiguration.Object, _mockProducerLogger.Object);
            var messages = new[] { "Message 1", "Message 2", "Message 3" };

            // Act & Assert
            foreach (var message in messages)
            {
                var exception = await Record.ExceptionAsync(() => producer.ProduceAsync(message));
                exception.Should().BeNull();
            }
        }

        [Fact]
        public async Task ProducerWrapper_LargeMessage_HandledCorrectly()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConfiguration.Object, _mockProducerLogger.Object);
            var largeMessage = new string('A', 10000); // 10KB message

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => producer.ProduceAsync(largeMessage));
            exception.Should().BeNull();
        }

        [Fact]
        public void ProducerWrapper_Dispose_Success()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConfiguration.Object, _mockProducerLogger.Object);

            // Act & Assert
            var exception = Record.Exception(() => producer.Dispose());
            exception.Should().BeNull();
        }

        [Fact]
        public void ConsumerWrapper_Dispose_Success()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConfiguration.Object, _mockConsumerLogger.Object);

            // Act & Assert
            var exception = Record.Exception(() => consumer.Dispose());
            exception.Should().BeNull();
        }
    }
}