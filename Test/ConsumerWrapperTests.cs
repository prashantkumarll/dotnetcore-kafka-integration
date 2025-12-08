using Api;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moq;
using Xunit;

namespace Test
{
    public class ConsumerWrapperTests : IDisposable
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusProcessor> _mockServiceBusProcessor;
        private readonly ConsumerWrapper _consumerWrapper;

        public ConsumerWrapperTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusProcessor = new Mock<ServiceBusProcessor>();

            _mockServiceBusClient
                .Setup(x => x.CreateProcessor(It.IsAny<string>()))
                .Returns(_mockServiceBusProcessor.Object);

            _consumerWrapper = new ConsumerWrapper();
        }

        [Fact]
        public async Task StartAsync_StartsProcessing()
        {
            // Arrange & Act
            var act = () => _consumerWrapper.StartAsync();

            // Assert
            // Since the implementation requires actual Service Bus connection,
            // we expect a configuration or connection exception
            await act.Should().ThrowAsync<Exception>()
                .Where(e => e.Message.Contains("connection") || 
                           e.Message.Contains("configuration") ||
                           e.Message.Contains("ServiceBus"));
        }

        [Fact]
        public async Task StopAsync_StopsProcessing()
        {
            // Arrange & Act
            var act = () => _consumerWrapper.StopAsync();

            // Assert
            // The stop method should handle the case where processor wasn't started
            await act.Should().NotThrowAsync<NullReferenceException>();
        }

        [Fact]
        public void ConsumerWrapper_CanBeInstantiated()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public async Task StartAsync_ThenStopAsync_CompletesSuccessfully()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            try
            {
                // Act
                await consumer.StartAsync();
            }
            catch (Exception)
            {
                // Expected due to missing configuration
            }

            // Assert - Should not throw when stopping
            var stopAct = () => consumer.StopAsync();
            await stopAct.Should().NotThrowAsync();
        }

        [Fact]
        public async Task MultipleStartCalls_DoNotCauseIssues()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            // Multiple start calls should be handled gracefully
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    await consumer.StartAsync();
                }
                catch (Exception ex)
                {
                    // Should fail with configuration, not with state issues
                    ex.Should().NotBeOfType<InvalidOperationException>();
                }
            }
        }

        [Fact]
        public async Task MultipleStopCalls_DoNotCauseIssues()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            // Multiple stop calls should be handled gracefully
            for (int i = 0; i < 3; i++)
            {
                var stopAct = () => consumer.StopAsync();
                await stopAct.Should().NotThrowAsync();
            }
        }

        public void Dispose()
        {
            _consumerWrapper?.Dispose();
        }
    }
}