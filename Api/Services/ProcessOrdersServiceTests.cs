using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Api.Services;
using Api.Models;
using Newtonsoft.Json;

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
        }

        [Fact]
        public void Constructor_WithNullProcessorOptions_ShouldThrow()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(null, _serviceBusClientMock.Object);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullServiceBusClient_ShouldThrow()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(_processorOptions, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithBothConfigsNull_ShouldThrow()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(null, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_WithValidToken_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);
            var cancellationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await service.StartAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithValidToken_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);
            var cancellationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await service.StopAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_WithCancelledToken_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            Func<Task> act = async () => await service.StartAsync(cts.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_AfterStart_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);
            var cancellationToken = new CancellationToken();

            // Act
            await service.StartAsync(cancellationToken);
            Func<Task> act = async () => await service.StopAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }
}