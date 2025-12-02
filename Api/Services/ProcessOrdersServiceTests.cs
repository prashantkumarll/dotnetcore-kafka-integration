using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Services;
using Api.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
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
                MaxConcurrentCalls = 1
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
            service.Should().BeOfType<ProcessOrdersService>();
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
        public void ProcessOrdersService_ShouldInheritFromBackgroundService()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);

            // Assert
            service.Should().BeAssignableTo<BackgroundService>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldImplementIHostedService()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_processorOptions, _serviceBusClientMock.Object);

            // Assert
            service.Should().BeAssignableTo<IHostedService>();
        }

        [Fact]
        public void ProcessOrdersService_WithEmptyBootstrapServers_ShouldCreateInstance()
        {
            // Arrange
            var emptyProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1
            };
            var emptyServiceBusClientMock = new Mock<ServiceBusClient>();

            // Act
            var service = new ProcessOrdersService(emptyProcessorOptions, emptyServiceBusClientMock.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_WithMinimalConfigs_ShouldCreateInstance()
        {
            // Arrange
            var minimalProcessorOptions = new ServiceBusProcessorOptions();
            var minimalServiceBusClientMock = new Mock<ServiceBusClient>();

            // Act
            var service = new ProcessOrdersService(minimalProcessorOptions, minimalServiceBusClientMock.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }
    }
}
}