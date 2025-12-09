using Xunit;
using FluentAssertions;
using Api;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class ProducerWrapperTests
    {
        private readonly string _mockConnectionString = "mock-connection-string";
        private readonly string _mockTopicName = "mock-topic";

        [Fact]
        public void ProducerWrapper_ShouldBeInstantiated_WithValidParameters()
        {
            // Arrange & Act
            var wrapper = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Should().BeOfType<ProducerWrapper>();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var method = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var method = typeof(ProducerWrapper).GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var method = typeof(ProducerWrapper).GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange & Act
            var wrapper = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            wrapper.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange & Act
            var action = () => new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task ProducerWrapper_DisposeAsync_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var action = async () => await wrapper.DisposeAsync();

            // Assert
            await action.Should().NotThrowAsync();
        }
    }
}