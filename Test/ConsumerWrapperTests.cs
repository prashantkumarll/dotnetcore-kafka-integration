using Api;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_ShouldBeInstantiable()
        {
            // Act
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().NotBeNull();
            consumer.Should().BeOfType<ConsumerWrapper>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Act
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Act
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().BeAssignableTo<IAsyncDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_Dispose_ShouldNotThrowException()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            var act = () => consumer.Dispose();
            act.Should().NotThrow();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsync_ShouldNotThrowException()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            var act = async () => await consumer.DisposeAsync();
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_ShouldExist()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var methodInfo = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            methodInfo.Should().NotBeNull();
            consumer.Should().BeOfType<ConsumerWrapper>();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsync_ShouldReturnValueTask()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act
            var task = consumer.DisposeAsync();

            // Assert
            task.Should().BeOfType<ValueTask>();
            await task;
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_ShouldBeAsyncMethod()
        {
            // Arrange
            var methodInfo = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            methodInfo.Should().NotBeNull();
            methodInfo.ReturnType.Should().BeAssignableTo<Task>();
        }
    }
}