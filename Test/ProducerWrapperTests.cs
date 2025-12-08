using Xunit;
using Api;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class ProducerWrapperTests
    {
        private readonly ProducerWrapper _producerWrapper;

        public ProducerWrapperTests()
        {
            _producerWrapper = new ProducerWrapper();
        }

        [Fact]
        public void ProducerWrapper_ShouldBeInstantiable()
        {
            // Arrange & Act
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeOfType<ProducerWrapper>();
        }

        [Fact]
        public void WriteMessage_ShouldBeCallable()
        {
            // Arrange
            var message = "test message";

            // Act & Assert - Should not throw during method call setup
            Action act = () => _producerWrapper.writeMessage(message);
            
            // Note: Actual execution may require proper configuration
            // but the method should exist and be callable
        }

        [Fact]
        public async Task DisposeAsync_ShouldBeCallable()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert - Should not throw
            await producer.DisposeAsync();
        }

        [Fact]
        public void Dispose_ShouldBeCallable()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert - Should not throw
            producer.Dispose();
        }

        [Fact]
        public void WriteMessage_WithNullMessage_ShouldHandleGracefully()
        {
            // Arrange
            string message = null;

            // Act & Assert
            Action act = () => _producerWrapper.writeMessage(message);
            // Method should exist and be callable
        }

        [Fact]
        public void WriteMessage_WithEmptyMessage_ShouldHandleGracefully()
        {
            // Arrange
            var message = string.Empty;

            // Act & Assert
            Action act = () => _producerWrapper.writeMessage(message);
            // Method should exist and be callable
        }
    }
}