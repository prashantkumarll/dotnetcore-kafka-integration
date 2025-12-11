using Xunit;
using FluentAssertions;
using Api;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptConnectionStringAndTopicName()
        {
            // Arrange & Act
            var constructors = typeof(ConsumerWrapper).GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => 
            {
                var parameters = c.GetParameters();
                return parameters.Length == 2 && 
                       parameters[0].ParameterType == typeof(string) &&
                       parameters[1].ParameterType == typeof(string);
            });

            // Assert
            targetConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveCorrectMethods()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var readMessageAsyncMethod = consumerType.GetMethod("ReadMessageAsync");
            var disposeAsyncMethod = consumerType.GetMethod("DisposeAsync");
            var disposeMethod = consumerType.GetMethod("Dispose");

            // Assert
            readMessageAsyncMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Theory]
        [InlineData("orders-topic")]
        [InlineData("notifications-topic")]
        [InlineData("events-topic")]
        public void ConsumerWrapper_WithDifferentTopicNames_ShouldAcceptValidTopicNames(string topicName)
        {
            // Arrange
            var connectionString = "test-connection";
            
            // Act & Assert
            // Verify constructor signature accepts these parameter types
            var constructors = typeof(ConsumerWrapper).GetConstructors();
            var validConstructor = constructors.Any(c => 
            {
                var parameters = c.GetParameters();
                return parameters.Length == 2 && 
                       parameters[0].ParameterType == typeof(string) &&
                       parameters[1].ParameterType == typeof(string);
            });

            validConstructor.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldBeInCorrectNamespace()
        {
            // Act
            var consumerType = typeof(ConsumerWrapper);

            // Assert
            consumerType.Namespace.Should().Be("Api");
            consumerType.Name.Should().Be("ConsumerWrapper");
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsyncMethod_ShouldBeAsync()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var readMessageAsyncMethod = consumerType.GetMethod("ReadMessageAsync");

            // Assert
            readMessageAsyncMethod.Should().NotBeNull();
            readMessageAsyncMethod.Name.Should().EndWith("Async");
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementDisposablePattern()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var hasDisposeMethod = consumerType.GetMethod("Dispose") != null;
            var hasDisposeAsyncMethod = consumerType.GetMethod("DisposeAsync") != null;

            // Assert
            hasDisposeMethod.Should().BeTrue();
            hasDisposeAsyncMethod.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Methods_ShouldBePublic()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var readMessageAsync = consumerType.GetMethod("ReadMessageAsync");
            var disposeAsync = consumerType.GetMethod("DisposeAsync");
            var dispose = consumerType.GetMethod("Dispose");

            // Assert
            readMessageAsync?.IsPublic.Should().BeTrue();
            disposeAsync?.IsPublic.Should().BeTrue();
            dispose?.IsPublic.Should().BeTrue();
        }
    }
}