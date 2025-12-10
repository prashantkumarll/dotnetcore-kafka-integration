using Xunit;
using Api;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class ConsumerWrapperTests
    {
        private readonly string _mockConnectionString = "mock-connection-string";
        private readonly string _mockTopicName = "mock-topic-name";

        [Fact]
        public void ConsumerWrapper_Should_Initialize_With_Valid_Parameters()
        {
            // Act
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_Accept_Connection_String_And_Topic()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_Have_ReadMessageAsync_Method()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var method = consumer.GetType().GetMethod("ReadMessageAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public async Task ConsumerWrapper_ReadMessageAsync_Should_Be_Callable()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            var act = async () => await consumer.ReadMessageAsync();
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void ConsumerWrapper_Should_Be_Disposable()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public async Task ConsumerWrapper_Should_Be_Async_Disposable()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            consumer.Should().BeAssignableTo<IAsyncDisposable>();

            // Act & Assert
            var act = async () => await consumer.DisposeAsync();
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void ConsumerWrapper_Type_Should_Be_In_Correct_Namespace()
        {
            // Act
            var type = typeof(ConsumerWrapper);

            // Assert
            type.Namespace.Should().Be("Api");
            type.Name.Should().Be("ConsumerWrapper");
        }
    }
}