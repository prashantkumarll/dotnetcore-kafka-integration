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
        public void ProducerWrapper_Should_Be_Instantiated_With_Parameters()
        {
            // Arrange & Act
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);

            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeOfType<ProducerWrapper>();
        }

        [Fact]
        public void ProducerWrapper_Should_Have_WriteMessage_Method()
        {
            // Arrange
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Should_Have_DisposeAsync_Method()
        {
            // Arrange
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ProducerWrapper).GetMethod("DisposeAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Should_Have_Dispose_Method()
        {
            // Arrange
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ProducerWrapper).GetMethod("Dispose");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Should_Implement_IDisposable()
        {
            // Arrange & Act
            var producer = new ProducerWrapper(MockConnectionString, MockTopicName);

            // Assert
            producer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ProducerWrapper_Constructor_Should_Accept_Valid_Parameters()
        {
            // Arrange & Act
            var producer = new ProducerWrapper("test-connection", "test-topic");

            // Assert
            producer.Should().NotBeNull();
            producer.GetType().Name.Should().Be("ProducerWrapper");
        }
    }
}