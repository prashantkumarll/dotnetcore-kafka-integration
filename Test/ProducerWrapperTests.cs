using Xunit;
using FluentAssertions;
using Api;
using System.Threading.Tasks;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Should_BeInstantiable()
        {
            // Arrange & Act
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_HaveWriteMessageMethod()
        {
            // Arrange & Act
            var method = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_HaveDisposeAsyncMethod()
        {
            // Arrange & Act
            var method = typeof(ProducerWrapper).GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_HaveDisposeMethod()
        {
            // Arrange & Act
            var method = typeof(ProducerWrapper).GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_BeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(ProducerWrapper);

            // Assert
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Should_ImplementIDisposable()
        {
            // Arrange & Act
            var producer = new ProducerWrapper();

            // Assert
            producer.Should().BeAssignableTo<System.IDisposable>();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsync_Should_NotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper();

            // Act & Assert
            await FluentActions.Invoking(async () => await producer.DisposeAsync())
                .Should().NotThrowAsync();
        }
    }
}