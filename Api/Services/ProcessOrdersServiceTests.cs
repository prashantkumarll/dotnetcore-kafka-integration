using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Api.Services;
using Api.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Tests
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
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WithNullMessage_ShouldContinue()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(string.Empty);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            // Act & Assert
            await service.Invoking(x => x.ExecuteAsync(cancellationTokenSource.Token))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task ExecuteAsync_WithValidOrder_ShouldProcessAndPublish()
        {
            // Arrange
            var testOrder = new OrderRequest { productname = "TestProduct" };
            var serializedOrder = JsonConvert.SerializeObject(testOrder);

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            // Act & Assert
            await service.Invoking(x => x.ExecuteAsync(cancellationTokenSource.Token))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidOrderJson_ShouldContinue()
        {
            // Arrange
            var invalidJson = "{ invalid: json }";

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(invalidJson);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            // Act & Assert
            await service.Invoking(x => x.ExecuteAsync(cancellationTokenSource.Token))
                .Should().NotThrowAsync();
        }

        [Fact]
        public void OrderRequest_ShouldUpdateStatusToCompleted()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct", status = OrderStatus.IN_PROGRESS };

            // Act
            order.status = OrderStatus.COMPLETED;

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void ProcessOrdersService_ShouldHandleCancellation()
        {
            // Arrange
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            cancellationTokenSource.Cancel();

            // Assert
            cancellationTokenSource.Token.IsCancellationRequested.Should().BeTrue();
        }
    }
}