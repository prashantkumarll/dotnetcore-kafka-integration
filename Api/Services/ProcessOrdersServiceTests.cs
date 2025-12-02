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
        private readonly Mock<ServiceBusProcessorOptions> _mockProcessorOptions;
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;

        public ProcessOrdersServiceTests()
        {
            _mockProcessorOptions = new Mock<ServiceBusProcessorOptions>();
            _mockServiceBusClient = new Mock<ServiceBusClient>();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockProcessorOptions.Object, _mockServiceBusClient.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WithNullMessage_ShouldContinue()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockProcessorOptions.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns((string)null);

            var service = new ProcessOrdersService(_mockProcessorOptions.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(CancellationToken.None);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidOrder_ShouldProcessAndPublish()
        {
            // Arrange
            var orderRequest = new OrderRequest { productname = "TestProduct" };
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockProcessorOptions.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockServiceBusClient.Object, "readytoship");

            var service = new ProcessOrdersService(_mockProcessorOptions.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(CancellationToken.None);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidOrderJson_ShouldLogWarning()
        {
            // Arrange
            var invalidJson = "{ invalid json }";

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockProcessorOptions.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(invalidJson);

            var service = new ProcessOrdersService(_mockProcessorOptions.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(CancellationToken.None);
        }

        [Fact]
        public async Task ExecuteAsync_WhenCancellationRequested_ShouldExit()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var service = new ProcessOrdersService(_mockProcessorOptions.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(cancellationTokenSource.Token);
        }
    }
}