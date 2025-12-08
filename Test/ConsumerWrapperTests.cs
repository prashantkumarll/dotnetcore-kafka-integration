using Xunit;
using Api;
using FluentAssertions;
using System.Threading.Tasks;

namespace Test
{
    public class ConsumerWrapperTests
    {
        private readonly ConsumerWrapper _consumerWrapper;

        public ConsumerWrapperTests()
        {
            _consumerWrapper = new ConsumerWrapper();
        }

        [Fact]
        public void ConsumerWrapper_ShouldBeInstantiable()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().NotBeNull();
            consumer.Should().BeOfType<ConsumerWrapper>();
        }

        [Fact]
        public async Task ReadMessageAsync_ShouldBeCallable()
        {
            // Arrange & Act
            var result = await _consumerWrapper.ReadMessageAsync();

            // Assert
            // Method should complete without throwing
            // Result type will depend on actual implementation
        }

        [Fact]
        public async Task DisposeAsync_ShouldBeCallable()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert - Should not throw
            await consumer.DisposeAsync();
        }

        [Fact]
        public void Dispose_ShouldBeCallable()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert - Should not throw
            consumer.Dispose();
        }

        [Fact]
        public async Task ReadMessageAsync_MultipleReads_ShouldWork()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act
            var result1 = await consumer.ReadMessageAsync();
            var result2 = await consumer.ReadMessageAsync();

            // Assert
            // Both calls should complete successfully
        }

        [Fact]
        public async Task ConsumerWrapper_ShouldSupportAsyncDisposalPattern()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act
            await consumer.ReadMessageAsync();
            await consumer.DisposeAsync();

            // Assert
            // Should complete without issues
        }
    }
}