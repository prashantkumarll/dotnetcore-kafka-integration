using Xunit;
using Api;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class ProducerWrapperTests
    {
        private readonly string _mockConnectionString = "mock-connection-string";
        private readonly string _mockTopicName = "mock-topic-name";

        [Fact]
        public void ProducerWrapper_Should_Initialize_With_Valid_Parameters()
        {
            // Act
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_Accept_Connection_String_And_Topic()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_Have_WriteMessage_Method()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act
            var method = producer.GetType().GetMethod("writeMessage");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_Be_Disposable()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public async Task ProducerWrapper_Should_Be_Async_Disposable()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Assert
            producer.Should().BeAssignableTo<IAsyncDisposable>();

            // Act & Assert
            var act = async () => await producer.DisposeAsync();
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void ProducerWrapper_Dispose_Should_Not_Throw()
        {
            // Arrange
            var producer = new ProducerWrapper(_mockConnectionString, _mockTopicName);

            // Act & Assert
            var act = () => producer.Dispose();
            act.Should().NotThrow();
        }
    }
}