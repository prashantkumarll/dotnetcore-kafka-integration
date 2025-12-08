using Api;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Test
{
    public class ConsumerWrapperTests
    {
        private readonly ConsumerWrapper _consumer;

        public ConsumerWrapperTests()
        {
            _consumer = new ConsumerWrapper();
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
        public async Task ReadMessageAsync_ExecutesWithoutException()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _consumer.ReadMessageAsync());
            exception.Should().BeNull();
        }

        [Fact]
        public async Task ReadMessageAsync_ReturnsTask()
        {
            // Act
            var result = _consumer.ReadMessageAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task>();
        }

        [Fact]
        public async Task DisposeAsync_ExecutesWithoutException()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _consumer.DisposeAsync().AsTask());
            exception.Should().BeNull();
        }

        [Fact]
        public void Dispose_ExecutesWithoutException()
        {
            // Act & Assert
            var exception = Record.Exception(() => _consumer.Dispose());
            exception.Should().BeNull();
        }

        [Fact]
        public async Task ConsumerWrapper_ImplementsDisposablePatterns()
        {
            // Arrange
            using var consumer = new ConsumerWrapper();

            // Act & Assert - Should not throw when disposed
            var exception = Record.Exception(() => consumer.Dispose());
            exception.Should().BeNull();
        }
    }
}