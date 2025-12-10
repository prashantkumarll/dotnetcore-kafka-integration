using Xunit;
using Api;
using FluentAssertions;
using System.Threading.Tasks;
using System;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Should_Be_Instantiable_With_Connection_String_And_Topic()
        {
            // Arrange
            var connectionString = "test-connection-string";
            var topicName = "test-topic";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_Should_Accept_String_Parameters()
        {
            // Arrange
            var connectionString = "mock-connection-string";
            var topicName = "mock-topic-name";

            // Act & Assert
            var action = () => new ConsumerWrapper(connectionString, topicName);
            action.Should().NotThrow();
        }

        [Fact]
        public void ConsumerWrapper_Should_Have_ReadMessageAsync_Method()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_Have_DisposeAsync_Method()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_Have_Dispose_Method()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Should_Be_In_Api_Namespace()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.GetType().Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Should_Implement_IDisposable()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);

            // Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_Should_Return_Task()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            method.Should().NotBeNull();
            method.ReturnType.Should().BeAssignableTo<Task>();
        }
    }
}