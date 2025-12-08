using Api;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

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

            // Act & Assert
            var act = () => producer.Dispose();
            act.Should().NotThrow();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsync_ShouldNotThrowException()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert
            var act = async () => await producer.DisposeAsync();
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void ProducerWrapper_WriteMessage_ShouldExist()
        {
            // Arrange
            var producer = new ProducerWrapper();
            var methodInfo = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Assert
            methodInfo.Should().NotBeNull();
            producer.Should().BeOfType<ProducerWrapper>();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsync_ShouldReturnValueTask()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act
            var task = producer.DisposeAsync();

            // Assert
            task.Should().BeOfType<ValueTask>();
            await task;
        }
    }
}