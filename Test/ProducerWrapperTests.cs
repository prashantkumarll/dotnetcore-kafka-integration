using System;
using System.Threading.Tasks;
using Api;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests
    {
        private const string MockConnectionString = "mock-connection-string";
        private const string MockTopicName = "mock-topic";

        [Fact]
        public void ProducerWrapper_ShouldBeInitializedWithParameters()
        {
            // Arrange & Act
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Act & Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange & Act
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);

            // Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ProducerWrapper).GetMethod("DisposeAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_WithEmptyConnectionString_ShouldNotThrow()
        {
            // Arrange & Act
            Action act = () => new ProducerWrapper("", MockTopicName);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ProducerWrapper_WithEmptyTopicName_ShouldNotThrow()
        {
            // Arrange & Act
            Action act = () => new ProducerWrapper(MockConnectionString, "");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ProducerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);

            // Act & Assert
            Action act = () => producer.Dispose();
            act.Should().NotThrow();
        }
    }
}