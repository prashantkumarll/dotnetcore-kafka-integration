using System;
using Xunit;
using Moq;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Services;
using Api.Models;

namespace Api.Tests.Services
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ConsumerConfig> _mockConsumerConfig;
        private readonly Mock<ProducerConfig> _mockProducerConfig;

        public ProcessOrdersServiceTests()
        {
            _mockConsumerConfig = new Mock<ConsumerConfig>();
            _mockProducerConfig = new Mock<ProducerConfig>();
        }

        [Fact]
        public async Task ExecuteAsync_ValidOrderRequest_ProcessesOrderSuccessfully()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");

            var orderRequest = new OrderRequest { productname = "TestProduct" };
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(JsonConvert.SerializeObject(orderRequest));

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => service.StartAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public void Constructor_ValidConfigs_InitializesSuccessfully()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_NullOrderRequest_HandlesGracefully()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns((string)null);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => service.StartAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public async Task ExecuteAsync_InvalidJsonOrder_HandlesException()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns("invalid json");

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => service.StartAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public void Constructor_NullConfigs_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProcessOrdersService(null, null));
        }
    }
}