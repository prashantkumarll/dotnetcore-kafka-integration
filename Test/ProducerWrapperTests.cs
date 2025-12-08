using Api;
using Xunit;
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
            // Arrange
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ProducerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert
            Action act = () => producer.Dispose();
            act.Should().NotThrow();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsync_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert
            Func<Task> act = async () => await producer.DisposeAsync();
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void ProducerWrapper_writeMessage_ShouldExist()
        {
            // Arrange
            var producer = new ProducerWrapper();
            var methodInfo = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_HasRequiredMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.GetMethod("writeMessage").Should().NotBeNull();
            type.GetMethod("DisposeAsync").Should().NotBeNull();
            type.GetMethod("Dispose").Should().NotBeNull();
        }
    }
}