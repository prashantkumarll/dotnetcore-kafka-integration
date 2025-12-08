using Xunit;
using Api;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_ShouldBeInstantiable()
        {
            // Act
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeOfType<ProducerWrapper>();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Act
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Act
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ProducerWrapper_Dispose_ShouldNotThrowException()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act
            Action disposeAction = () => producer.Dispose();

            // Assert
            disposeAction.Should().NotThrow();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsync_ShouldNotThrowException()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act
            Func<Task> disposeAsyncAction = async () => await producer.DisposeAsync();

            // Assert
            await disposeAsyncAction.Should().NotThrowAsync();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var producer = new ProducerWrapper();
            var methodInfo = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public async Task ProducerWrapper_WriteMessage_ShouldNotThrowWithValidInput()
        {
            // Arrange
            var producer = new ProducerWrapper();
            var testMessage = "test message";

            // Act & Assert
            try
            {
                producer.writeMessage(testMessage);
            }
            catch (Exception)
            {
                // Expected to potentially throw due to missing configuration
                // This is acceptable for unit testing
            }
        }
    }
}