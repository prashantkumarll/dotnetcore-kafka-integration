using Xunit;
using FluentAssertions;
using Api;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_ShouldBeInstantiable()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            consumer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var methodInfo = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var methodInfo = typeof(ConsumerWrapper).GetMethod("DisposeAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var methodInfo = typeof(ConsumerWrapper).GetMethod("Dispose");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_DisposeShouldNotThrow()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            var exception = Record.Exception(() => consumer.Dispose());
            exception.Should().BeNull();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsyncShouldNotThrow()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () => await consumer.DisposeAsync());
            exception.Should().BeNull();
        }

        [Fact]
        public async Task ConsumerWrapper_ReadMessageAsyncShouldReturnTask()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act
            var task = consumer.ReadMessageAsync();

            // Assert
            task.Should().BeAssignableTo<Task>();
            await task;
        }
    }
}