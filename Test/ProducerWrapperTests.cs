using Xunit;
using Api;
using FluentAssertions;
using System.Threading.Tasks;
using System;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Should_Be_Instantiable_With_Connection_String_And_Topic()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Constructor_Should_Accept_String_Parameters()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";

            // Act & Assert
            var action = () => new ProducerWrapper(connectionString, topicName);
            action.Should().NotThrow();
        }

        [Fact]
        public void ProducerWrapper_Should_Have_WriteMessage_Method()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var producer = new ProducerWrapper(connectionString, topicName);

            // Act
            var method = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_Have_DisposeAsync_Method()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var method = typeof(ProducerWrapper).GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_Have_Dispose_Method()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var method = typeof(ProducerWrapper).GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Should_Be_In_Api_Namespace()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.GetType().Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Should_Implement_IDisposable()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var producer = new ProducerWrapper(connectionString, topicName);

            // Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }
    }
}