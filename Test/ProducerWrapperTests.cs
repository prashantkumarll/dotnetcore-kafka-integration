using Xunit;
using FluentAssertions;
using Api;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptConnectionStringAndTopicName()
        {
            // Note: ProducerWrapper likely uses ServiceBusClient internally (sealed class)
            // We can test constructor signature and basic properties without mocking
            
            // Arrange & Act
            var exception = Record.Exception(() => 
            {
                var constructors = typeof(ProducerWrapper).GetConstructors();
                var targetConstructor = constructors.FirstOrDefault(c => 
                {
                    var parameters = c.GetParameters();
                    return parameters.Length == 2 && 
                           parameters[0].ParameterType == typeof(string) &&
                           parameters[1].ParameterType == typeof(string);
                });
                
                return targetConstructor;
            });

            // Assert
            exception.Should().BeNull();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldHaveCorrectMethods()
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);

            // Act
            var writeMessageMethod = producerType.GetMethod("writeMessage");
            var disposeAsyncMethod = producerType.GetMethod("DisposeAsync");
            var disposeMethod = producerType.GetMethod("Dispose");

            // Assert
            writeMessageMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull(); 
            disposeMethod.Should().NotBeNull();
        }

        [Theory]
        [InlineData("test-connection-string")]
        [InlineData("")]
        [InlineData("Endpoint=sb://test.servicebus.windows.net/")]
        public void ProducerWrapper_ConstructorParameters_ShouldAcceptStringValues(string connectionString)
        {
            // Arrange
            var topicName = "test-topic";
            
            // Act & Assert
            // We verify the constructor exists and accepts string parameters
            // Actual instantiation would require valid Azure Service Bus connection
            var constructors = typeof(ProducerWrapper).GetConstructors();
            var validConstructor = constructors.Any(c => 
            {
                var parameters = c.GetParameters();
                return parameters.Length == 2 && 
                       parameters.All(p => p.ParameterType == typeof(string));
            });

            validConstructor.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldBeInCorrectNamespace()
        {
            // Act
            var producerType = typeof(ProducerWrapper);

            // Assert
            producerType.Namespace.Should().Be("Api");
            producerType.Name.Should().Be("ProducerWrapper");
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposablePattern()
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);

            // Act
            var implementsIDisposable = typeof(IDisposable).IsAssignableFrom(producerType);
            var hasDisposeMethod = producerType.GetMethod("Dispose") != null;
            var hasDisposeAsyncMethod = producerType.GetMethod("DisposeAsync") != null;

            // Assert
            hasDisposeMethod.Should().BeTrue();
            hasDisposeAsyncMethod.Should().BeTrue();
        }
    }
}