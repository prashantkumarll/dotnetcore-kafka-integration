using System.Threading.Tasks;
using Api;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class ConsumerWrapperTests
    {
        private readonly string _connectionString = "mock-connection-string";
        private readonly string _topicName = "mock-topic";

        [Fact]
        public void ConsumerWrapper_ShouldInstantiateSuccessfully()
        {
            // Act
            var consumer = new ConsumerWrapper(_connectionString, _topicName);

            // Assert
            consumer.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveCorrectConstructor()
        {
            // Act
            var constructors = typeof(ConsumerWrapper).GetConstructors();

            // Assert
            constructors.Should().HaveCount(1);
            var constructor = constructors[0];
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[1].ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_connectionString, _topicName);

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("ReadMessageAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_connectionString, _topicName);

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var consumer = new ConsumerWrapper(_connectionString, _topicName);

            // Act
            var method = typeof(ConsumerWrapper).GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldBeInCorrectNamespace()
        {
            // Act
            var type = typeof(ConsumerWrapper);

            // Assert
            type.Namespace.Should().Be("Api");
        }
    }
}