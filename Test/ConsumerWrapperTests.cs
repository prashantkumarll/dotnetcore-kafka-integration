using System;
using System.Threading.Tasks;
using Api;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class ConsumerWrapperTests
    {
        private const string MockConnectionString = "mock-connection-string";
        private const string MockTopicName = "mock-topic";

        [Fact]
        public void ConsumerWrapper_Should_Be_Instantiated_With_Parameters()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);

            // Assert
            consumer.Should().NotBeNull();
            consumer.Should().BeOfType<ConsumerWrapper>();
        }

        [Fact]
        public void ConsumerWrapper_Should_Have_ReadMessageAsync_Method()
        {
            // Arrange
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Should_Have_DisposeAsync_Method()
        {
            // Arrange
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ConsumerWrapper).GetMethod("DisposeAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Should_Have_Dispose_Method()
        {
            // Arrange
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ConsumerWrapper).GetMethod("Dispose");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Should_Implement_IDisposable()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);

            // Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_Should_Accept_Different_Parameters()
        {
            // Arrange & Act
            var consumer1 = new ConsumerWrapper("connection1", "topic1");
            var consumer2 = new ConsumerWrapper("connection2", "topic2");

            // Assert
            consumer1.Should().NotBeNull();
            consumer2.Should().NotBeNull();
            consumer1.Should().NotBeSameAs(consumer2);
        }
    }
}