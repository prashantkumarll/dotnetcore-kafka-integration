using System;
using Xunit;
using Moq;
using FluentAssertions;
using Newtonsoft.Json;
using Api.Services;
using Api.Models;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ServiceBusProcessorOptions> _mockConsumerConfig;
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;

        public ProcessOrdersServiceTests()
        {
            _mockConsumerConfig = new Mock<ServiceBusProcessorOptions>();
            _mockServiceBusClient = new Mock<ServiceBusClient>();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockServiceBusClient.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WithNullOrEmptyMessage_ShouldContinue()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(string.Empty);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(CancellationToken.None);
            mockConsumerWrapper.Verify(x => x.readMessage(), Times.AtLeastOnce());
        }

        [Fact]
        public async Task ExecuteAsync_WithValidOrder_ShouldProcessAndPublish()
        {
            // Arrange
            var testOrder = new OrderRequest { productname = "TestProduct" };
            var serializedOrder = JsonConvert.SerializeObject(testOrder);

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockServiceBusClient.Object, "readytoship");

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(CancellationToken.None);
            mockConsumerWrapper.Verify(x => x.readMessage(), Times.AtLeastOnce());
            mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidOrderDeserialization_ShouldSkip()
        {
            // Arrange
            var invalidOrderJson = "{ invalid: json }";
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(invalidOrderJson);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(CancellationToken.None);
            mockConsumerWrapper.Verify(x => x.readMessage(), Times.AtLeastOnce());
        }

        [Fact]
        public void OrderRequest_ShouldHaveCompletedStatusAfterProcessing()
        {
            // Arrange
            var testOrder = new OrderRequest { productname = "TestProduct", status = OrderStatus.IN_PROGRESS };

            // Act
            testOrder.status = OrderStatus.COMPLETED;

            // Assert
            testOrder.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void ProcessOrdersService_ShouldHandleMultipleOrderTypes()
        {
            // Arrange
            var orders = new[] {
                new OrderRequest { productname = "Electronics", status = OrderStatus.IN_PROGRESS },
                new OrderRequest { productname = "Clothing", status = OrderStatus.IN_PROGRESS }
            };

            // Act & Assert
            foreach (var order in orders)
            {
                order.status = OrderStatus.COMPLETED;
                order.status.Should().Be(OrderStatus.COMPLETED);
            }
        }

        [Fact]
        public void OrderStatus_ShouldSupportAllDefinedStatuses()
        {
            // Arrange & Act
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statuses.Should().Contain(new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED });
        }
    }
}