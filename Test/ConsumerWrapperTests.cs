using System;
using System.Threading.Tasks;
using Api;
using Xunit;
using FluentAssertions;

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
        public void ConsumerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            Action act = () => consumer.Dispose();
            act.Should().NotThrow();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsync_ShouldNotThrow()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            Func<Task> act = async () => await consumer.DisposeAsync();
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
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveCorrectNamespace()
        {
            // Act
            var type = typeof(ConsumerWrapper);

            // Assert
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public async Task ConsumerWrapper_ReadMessageAsync_ShouldReturnTask()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            Func<Task> act = async () => await consumer.ReadMessageAsync();
            await act.Should().NotThrowAsync();
        }
    }
}