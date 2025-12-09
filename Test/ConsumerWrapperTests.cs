using Xunit;
using FluentAssertions;
using Api;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class ConsumerWrapperTests
    {
        private readonly string _mockConnectionString = "mock-connection-string";
        private readonly string _mockTopicName = "mock-topic";

        [Fact]
        public void ConsumerWrapper_ShouldBeInstantiated_WithValidParameters()
        {
            // Arrange & Act
            var wrapper = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Should().BeOfType<ConsumerWrapper>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange & Act
            var wrapper = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            wrapper.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public async Task ConsumerWrapper_DisposeAsync_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var action = async () => await wrapper.DisposeAsync();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ConsumerWrapper_ReadMessageAsync_ShouldNotThrow()
        {
            // Arrange
            var wrapper = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var action = async () => await wrapper.ReadMessageAsync();

            // Assert
            await action.Should().NotThrowAsync();
        }
    }
}