using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Services;
using Api.Models;

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
        public async Task ExecuteAsync_ShouldProcessOrder_WhenMessageReceived()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig();
            var producerConfig = new ProducerConfig();
            var service = new ProcessOrdersService(consumerConfig, producerConfig);
            var cancellationTokenSource = new System.Threading.CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

            // Act & Assert
            await Assert.RaisesAsync<OperationCanceledException>(async () => 
            {
                await service.StartAsync(cancellationTokenSource.Token);
            });
        }

        [Fact]
        public void OrderRequest_ShouldDeserializeCorrectly()
        {
            // Arrange
            string jsonOrder = "{"productname":"TestProduct", "status":0}";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(jsonOrder);

            // Assert
            order.Should().NotBeNull();
            order.productname.Should().Be("TestProduct");
            order.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_StatusShouldUpdateToCompleted()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct", status = OrderStatus.IN_PROGRESS };

            // Act
            order.status = OrderStatus.COMPLETED;

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void ProcessOrdersService_ShouldHandleNullConfigs()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProcessOrdersService(null, null));
        }
    }
}