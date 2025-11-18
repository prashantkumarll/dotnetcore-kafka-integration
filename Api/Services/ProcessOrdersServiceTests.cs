using System;
using Xunit;
using Moq;
using Api.Services;
using Api.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public async Task ExecuteAsync_ValidOrderRequest_ProcessesOrderSuccessfully()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { GroupId = "test-group" };
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var mockConsumerWrapper = new Mock<IConsumerWrapper>();
            var mockProducerWrapper = new Mock<IProducerWrapper>();

            var orderRequest = new OrderRequest
            {
                productname = "TestProduct",
                status = OrderStatus.PENDING
            };

            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(JsonConvert.SerializeObject(orderRequest));

            var service = new ProcessOrdersService(consumerConfig, producerConfig);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            mockProducerWrapper.Verify(x => x.writeMessage(It.Is<string>(msg => 
                JsonConvert.DeserializeObject<OrderRequest>(msg).status == OrderStatus.COMPLETED)), Times.Once);
        }

        [Fact]
        public void Constructor_NullConfigs_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProcessOrdersService(null, null));
        }

        [Fact]
        public async Task ExecuteAsync_EmptyOrderRequest_HandlesGracefully()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { GroupId = "test-group" };
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var mockConsumerWrapper = new Mock<IConsumerWrapper>();

            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(string.Empty);

            var service = new ProcessOrdersService(consumerConfig, producerConfig);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(() => service.StartAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public async Task ExecuteAsync_CancellationRequested_StopsProcessing()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { GroupId = "test-group" };
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var cancellationTokenSource = new CancellationTokenSource();

            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Act
            cancellationTokenSource.Cancel();
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            // Verify service stops when cancellation is requested
        }

        [Fact]
        public void OrderRequest_InvalidData_ThrowsValidationException()
        {
            // Arrange
            var invalidOrderJson = "{\"productname\":\"\"}";

            // Act & Assert
            Assert.Throws<ValidationException>(() => JsonConvert.DeserializeObject<OrderRequest>(invalidOrderJson));
        }

        [Fact]
        public async Task ExecuteAsync_KafkaConnectionError_HandlesGracefully()
        {
            // Arrange
            var invalidConsumerConfig = new ConsumerConfig { BootstrapServers = "invalid-server" };
            var invalidProducerConfig = new ProducerConfig { BootstrapServers = "invalid-server" };

            var service = new ProcessOrdersService(invalidConsumerConfig, invalidProducerConfig);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act & Assert
            await Assert.ThrowsAsync<KafkaException>(() => service.StartAsync(cancellationTokenSource.Token));
        }
    }
}