using Api;
using Xunit;
using FluentAssertions;
using System;
using System.Threading.Tasks;

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
            // Arrange
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIAsyncDisposable()
        {
            // Arrange
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
        public async Task ConsumerWrapper_ReadMessageAsync_ShouldBeAsync()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act
            var result = consumer.ReadMessageAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task>();
        }

        [Fact]
        public void ConsumerWrapper_HasRequiredMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.GetMethod("ReadMessageAsync").Should().NotBeNull();
            type.GetMethod("DisposeAsync").Should().NotBeNull();
            type.GetMethod("Dispose").Should().NotBeNull();
        }
    }
}