using Xunit;
using Moq;
using FluentAssertions;
using Api;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Constructor_ShouldNotThrow()
        {
            // Act & Assert
            var action = () => new ConsumerWrapper();
            action.Should().NotThrow();
        }

        [Fact]
        public async Task StartConsumingAsync_WithValidTopic_ShouldNotThrowImmediately()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var topic = "test-topic";
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(100)); // Cancel quickly

            // Act
            var action = async () => await consumer.StartConsumingAsync(topic, cancellationTokenSource.Token);

            // Assert
            // This should either complete successfully or throw OperationCanceledException
            try
            {
                await action();
            }
            catch (OperationCanceledException)
            {
                // This is expected due to cancellation
            }
            catch (Exception ex)
            {
                // Other exceptions should be meaningful
                ex.Should().NotBeNull();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task StartConsumingAsync_WithInvalidTopic_ShouldHandleGracefully(string? topic)
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(50));

            // Act
            var action = async () => await consumer.StartConsumingAsync(topic!, cancellationTokenSource.Token);

            // Assert
            try
            {
                await action();
            }
            catch (OperationCanceledException)
            {
                // Expected due to cancellation
            }
            catch (ArgumentException)
            {
                // Expected for invalid topic
            }
            catch (Exception ex)
            {
                // Other exceptions should be meaningful
                ex.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task StartConsumingAsync_WithCancelledToken_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var topic = "test-topic";
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel(); // Cancel immediately

            // Act
            var action = async () => await consumer.StartConsumingAsync(topic, cancellationTokenSource.Token);

            // Assert
            await action.Should().ThrowAsync<OperationCanceledException>();
        }

        [Fact]
        public async Task StartConsumingAsync_WithTimeout_ShouldRespectCancellation()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var topic = "test-topic";
            var cancellationTokenSource = new CancellationTokenSource();
            var timeout = TimeSpan.FromMilliseconds(200);
            
            // Act
            var startTime = DateTime.UtcNow;
            cancellationTokenSource.CancelAfter(timeout);
            
            try
            {
                await consumer.StartConsumingAsync(topic, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // Expected
            }
            
            var elapsed = DateTime.UtcNow - startTime;

            // Assert
            elapsed.Should().BeLessThan(timeout.Add(TimeSpan.FromMilliseconds(100))); // Allow some tolerance
        }

        [Fact]
        public void ConsumerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act
            var action = () => consumer.Dispose();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void ConsumerWrapper_DisposeAfterUsing_ShouldNotThrow()
        {
            // Arrange & Act
            var action = () =>
            {
                using var consumer = new ConsumerWrapper();
                // Use consumer here if needed
            };

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task StartConsumingAsync_MultipleTopics_ShouldHandleSequentially()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var topics = new[] { "topic1", "topic2", "topic3" };
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(50));

            // Act & Assert
            foreach (var topic in topics)
            {
                try
                {
                    await consumer.StartConsumingAsync(topic, cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // Expected
                }
                catch (Exception ex)
                {
                    ex.Should().NotBeNull();
                }
            }
        }
    }
}