using System.Threading.Tasks;
using Api;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests
    {
        private readonly string _connectionString = "mock-connection-string";
        private readonly string _topicName = "mock-topic";

        [Fact]
        public void ProducerWrapper_ShouldInstantiateSuccessfully()
        {
            // Act
            var producer = new ProducerWrapper(_connectionString, _topicName);

            // Assert
            producer.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveCorrectConstructor()
        {
            // Act
            var constructors = typeof(ProducerWrapper).GetConstructors();

            // Assert
            constructors.Should().HaveCount(1);
            var constructor = constructors[0];
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[1].ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(_connectionString, _topicName);

            // Act
            var method = typeof(ProducerWrapper).GetMethod("writeMessage");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeAsyncMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(_connectionString, _topicName);

            // Act
            var method = typeof(ProducerWrapper).GetMethod("DisposeAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var producer = new ProducerWrapper(_connectionString, _topicName);

            // Act
            var method = typeof(ProducerWrapper).GetMethod("Dispose");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldBeInCorrectNamespace()
        {
            // Act
            var type = typeof(ProducerWrapper);

            // Assert
            type.Namespace.Should().Be("Api");
        }
    }
}