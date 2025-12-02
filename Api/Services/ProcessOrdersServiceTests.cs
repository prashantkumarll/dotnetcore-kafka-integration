using System;
using Xunit;
using Moq;
using FluentAssertions;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Api.Services;
using Api.Models;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ServiceBusProcessorOptions> _mockServiceBusProcessorOptions;
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;

        public ProcessOrdersServiceTests()
        {
            _mockServiceBusProcessorOptions = new Mock<ServiceBusProcessorOptions>();
            _mockServiceBusClient = new Mock<ServiceBusClient>();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockServiceBusProcessorOptions.Object, _mockServiceBusClient.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WithNullMessage_ShouldContinue()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockServiceBusProcessorOptions.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns((string)null);

            var service = new ProcessOrdersService(_mockServiceBusProcessorOptions.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await Assert.RaisesAsync<OperationCanceledException>(async () => 
            {
                await service.ExecuteAsync(new System.Threading.CancellationToken(true));
            });
        }

        [Fact]
        public void DeserializeOrder_WithValidJson_ShouldSucceed()
        {
            // Arrange
            var orderJson = "{"productname":"TestProduct", "status":0}";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(orderJson);

            // Assert
            order.Should().NotBeNull();
            order.productname.Should().Be("TestProduct");
            order.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void DeserializeOrder_WithInvalidJson_ShouldReturnNull()
        {
            // Arrange
            var invalidJson = "invalid json";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(invalidJson);

            // Assert
            order.Should().BeNull();
        }

        [Fact]
        public void OrderStatus_ShouldHaveExpectedValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statusValues.Should().Contain(OrderStatus.COMPLETED);
        }

        [Fact]
        public void ServiceBusClient_ShouldAllowConfiguration()
        {
            // Arrange
            var config = new ServiceBusClient
            {
                ConnectionString = "localhost:9092"
            };

            // Assert
            config.ConnectionString.Should().Be("localhost:9092");
        }

        [Fact]
        public void ServiceBusProcessorOptions_ShouldAllowConfiguration()
        {
            // Arrange
            var config = new ServiceBusProcessorOptions
            {
                ConnectionString = "localhost:9092",
                SessionId = "test-group"
            };

            // Assert
            config.ConnectionString.Should().Be("localhost:9092");
            config.SessionId.Should().Be("test-group");
        }

        [Fact]
        public void OrderRequest_ShouldSupportStatusChange()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct", status = OrderStatus.COMPLETED };

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}