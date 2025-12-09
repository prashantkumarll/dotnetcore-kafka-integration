using Xunit;
using FluentAssertions;
using Api;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_ShouldBeInstantiable()
        {
            // Arrange & Act
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert
            producer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var producer = new ProducerWrapper();
            var methodInfo = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var producer = new ProducerWrapper();
            var methodInfo = typeof(ProducerWrapper).GetMethod("DisposeAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var producer = new ProducerWrapper();
            var methodInfo = typeof(ProducerWrapper).GetMethod("Dispose");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_DisposeShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert
            var exception = Record.Exception(() => producer.Dispose());
            exception.Should().BeNull();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsyncShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () => await producer.DisposeAsync());
            exception.Should().BeNull();
        }
    }
}