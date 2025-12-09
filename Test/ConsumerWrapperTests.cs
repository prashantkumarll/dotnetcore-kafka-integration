using Xunit;
using FluentAssertions;
using Api;
using System.Threading.Tasks;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Should_BeInstantiable()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_HaveReadMessageAsyncMethod()
        {
            // Arrange & Act
            var method = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_HaveDisposeAsyncMethod()
        {
            // Arrange & Act
            var method = typeof(ConsumerWrapper).GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_HaveDisposeMethod()
        {
            // Arrange & Act
            var method = typeof(ConsumerWrapper).GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_BeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(ConsumerWrapper);

            // Assert
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Should_ImplementIDisposable()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper();

            // Assert
            consumer.Should().BeAssignableTo<System.IDisposable>();
        }

        [Fact]
        public async Task ConsumerWrapper_ReadMessageAsync_Should_NotThrow()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            await FluentActions.Invoking(async () => await consumer.ReadMessageAsync())
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsync_Should_NotThrow()
        {
            // Arrange
            var consumer = new ConsumerWrapper();

            // Act & Assert
            await FluentActions.Invoking(async () => await consumer.DisposeAsync())
                .Should().NotThrowAsync();
        }
    }
}