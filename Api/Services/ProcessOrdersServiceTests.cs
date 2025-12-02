using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Services;
using Api.Models;
using Microsoft.Extensions.Hosting;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly ServiceBusProcessorOptions _processorOptions;
        private readonly Mock<ServiceBusClient> _serviceBusClientMock;

        public ProcessOrdersServiceTests()
        {
            // Arrange - Setup test configurations
            _processorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            };

            _serviceBusClientMock = new Mock<ServiceBusClient>();
        }

        [Fact]
        public void Constructor_WithValidConfigs_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeAssignableTo<BackgroundService>();
        }

        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(null, _serviceBusClientMock.Object);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(_processorOptions, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithBothNullConfigs_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(null, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_WithValidService_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);
            var cancellationToken = CancellationToken.None;

            // Act
            Func<Task> act = async () => await service.StartAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithValidService_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);
            var cancellationToken = CancellationToken.None;

            // Act
            Func<Task> act = async () => await service.StopAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act
            Func<Task> act = async () => await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_AfterStart_ShouldCompleteSuccessfully()
        {
            // Arrange
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);
            var cancellationToken = CancellationToken.None;

            // Act
            await service.StartAsync(cancellationToken);
            
            // Small delay to allow service to initialize
            await Task.Delay(50);
            
            Func<Task> stopAct = async () => await service.StopAsync(cancellationToken);

            // Assert
            await stopAct.Should().NotThrowAsync();
        }
    }
}