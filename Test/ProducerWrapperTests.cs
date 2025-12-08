using Api;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Test
{
    public class ProducerWrapperTests
    {
        private readonly ProducerWrapper _producer;

        public ProducerWrapperTests()
        {
            _producer = new ProducerWrapper();
        }

        [Fact]
        public void ProducerWrapper_CanBeInstantiated()
        {
            // Arrange & Act
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void WriteMessage_ExecutesWithoutException()
        {
            // Arrange
            var message = "test message";

            // Act & Assert
            var exception = Record.Exception(() => _producer.writeMessage(message));
            exception.Should().BeNull();
        }

        [Fact]
        public void WriteMessage_WithNullMessage_HandlesGracefully()
        {
            // Arrange
            string message = null;

            // Act & Assert
            var exception = Record.Exception(() => _producer.writeMessage(message));
            exception.Should().BeNull();
        }

        [Fact]
        public void WriteMessage_WithEmptyMessage_HandlesGracefully()
        {
            // Arrange
            var message = string.Empty;

            // Act & Assert
            var exception = Record.Exception(() => _producer.writeMessage(message));
            exception.Should().BeNull();
        }

        [Fact]
        public async Task DisposeAsync_ExecutesWithoutException()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _producer.DisposeAsync().AsTask());
            exception.Should().BeNull();
        }

        [Fact]
        public void Dispose_ExecutesWithoutException()
        {
            // Act & Assert
            var exception = Record.Exception(() => _producer.Dispose());
            exception.Should().BeNull();
        }
    }
}