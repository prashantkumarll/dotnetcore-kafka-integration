using Api;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests
    {
        private const string TestConnectionString = "test-connection-string";
        private const string TestTopicName = "test-topic";

        [Fact]
        public void ProducerWrapper_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var producer = new ProducerWrapper(TestConnectionString, TestTopicName);

            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeOfType<ProducerWrapper>();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(TestConnectionString, TestTopicName);
            var methodInfo = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(TestConnectionString, TestTopicName);
            var methodInfo = typeof(ProducerWrapper).GetMethod("DisposeAsync");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(TestConnectionString, TestTopicName);
            var methodInfo = typeof(ProducerWrapper).GetMethod("Dispose");

            // Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var producer = new ProducerWrapper(TestConnectionString, TestTopicName);

            // Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_Constructor_WithEmptyStrings_ShouldNotThrow()
        {
            // Arrange & Act
            var action = () => new ProducerWrapper(string.Empty, string.Empty);

            // Assert
            action.Should().NotThrow();
        }
    }
}