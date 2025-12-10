using Api;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class ConsumerWrapperTests
    {
        private const string TestConnectionString = "test-connection-string";
        private const string TestTopicName = "test-topic";

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper(TestConnectionString, TestTopicName);

            // Assert
            consumer.Should().NotBeNull();
            consumer.Should().BeOfType<ConsumerWrapper>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(TestConnectionString, TestTopicName);
            var methodInfo = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(TestConnectionString, TestTopicName);
            var methodInfo = typeof(ConsumerWrapper).GetMethod("DisposeAsync");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(TestConnectionString, TestTopicName);
            var methodInfo = typeof(ConsumerWrapper).GetMethod("Dispose");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var consumer = new ConsumerWrapper(TestConnectionString, TestTopicName);

            // Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_WithNullParameters_ShouldNotThrow()
        {
            // Arrange & Act
            var action = () => new ConsumerWrapper(null, null);

            // Assert
            action.Should().NotThrow();
        }
    }
}