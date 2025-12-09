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
        public void ConsumerWrapper_ShouldBeInitializedWithParameters()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
            methodInfo.ReturnType.Should().Be(typeof(Task));
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);

            // Assert
            consumer.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);
            var methodInfo = typeof(ConsumerWrapper).GetMethod("DisposeAsync");

            // Act & Assert
            methodInfo.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_WithNullConnectionString_ShouldNotThrow()
        {
            // Arrange & Act
            Action act = () => new ConsumerWrapper(null, MockTopicName);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ConsumerWrapper_WithNullTopicName_ShouldNotThrow()
        {
            // Arrange & Act
            Action act = () => new ConsumerWrapper(MockConnectionString, null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ConsumerWrapper_Dispose_ShouldNotThrow()
        {
            // Arrange
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);

            // Act & Assert
            Action act = () => consumer.Dispose();
            act.Should().NotThrow();
        }

        [Fact]
        public async Task ReadMessageAsync_ShouldBeCallable()
        {
            // Arrange
            var consumer = new ConsumerWrapper(MockConnectionString, MockTopicName);

            // Act & Assert
            Func<Task> act = async () => await consumer.ReadMessageAsync();
            await act.Should().NotThrowAsync();
        }
    }
}