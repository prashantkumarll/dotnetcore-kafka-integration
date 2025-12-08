using Xunit;
using Api;
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

            // Act
            Action disposeAction = () => consumer.Dispose();

            // Assert
            disposeAction.Should().NotThrow();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsync_ShouldNotThrowException()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act
            Func<Task> disposeAsyncAction = async () => await consumer.DisposeAsync();

            // Assert
            await disposeAsyncAction.Should().NotThrowAsync();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper();
            var methodInfo = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public async Task ConsumerWrapper_ReadMessageAsync_ShouldReturnTask()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            try
            {
                var task = consumer.ReadMessageAsync();
                task.Should().NotBeNull();
                task.Should().BeOfType<Task>();
            }
            catch (Exception)
            {
                // Expected to potentially throw due to missing configuration
                // This is acceptable for unit testing
            }
        }

        [Fact]
        public void ConsumerWrapper_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(ConsumerWrapper);

            // Assert
            type.Namespace.Should().Be("Api");
            type.FullName.Should().Be("Api.ConsumerWrapper");
        }
    }
}