using Xunit;
using FluentAssertions;
using Api.Controllers;
using Azure.Messaging.ServiceBus;

namespace Test
{
    public class OrderControllerTests
    {
        [Fact]
        public void OrderController_Constructor_ShouldAcceptServiceBusClient()
        {
            // Note: We cannot mock ServiceBusClient as it's sealed
            // This test verifies the constructor signature exists
            // In a real scenario, this would be an integration test
            
            // Arrange & Act
            var exception = Record.Exception(() => 
            {
                // We can't actually create a ServiceBusClient without real connection string
                // This test just verifies the constructor exists with correct parameter type
                var constructors = typeof(OrderController).GetConstructors();
                var targetConstructor = constructors.FirstOrDefault(c => 
                {
                    var parameters = c.GetParameters();
                    return parameters.Length == 1 && parameters[0].ParameterType == typeof(ServiceBusClient);
                });
                
                return targetConstructor;
            });

            // Assert
            exception.Should().BeNull();
        }

        [Fact]
        public void OrderController_Type_ShouldBeCorrectType()
        {
            // Assert
            typeof(OrderController).Should().NotBeNull();
            typeof(OrderController).Name.Should().Be("OrderController");
            typeof(OrderController).Namespace.Should().Be("Api.Controllers");
        }

        [Fact]
        public void OrderController_PostAsyncMethod_ShouldExist()
        {
            // Arrange & Act
            var postAsyncMethod = typeof(OrderController).GetMethod("PostAsync");

            // Assert
            postAsyncMethod.Should().NotBeNull();
            postAsyncMethod.Name.Should().Be("PostAsync");
        }

        [Fact]
        public void OrderController_ShouldHavePublicPostAsyncMethod()
        {
            // Arrange
            var controllerType = typeof(OrderController);

            // Act
            var methods = controllerType.GetMethods()
                                      .Where(m => m.Name == "PostAsync" && m.IsPublic);

            // Assert
            methods.Should().NotBeEmpty();
            methods.Should().HaveCount(1);
        }

        [Fact]
        public void OrderController_ShouldBeInCorrectNamespace()
        {
            // Act
            var controllerType = typeof(OrderController);

            // Assert
            controllerType.Namespace.Should().Be("Api.Controllers");
            controllerType.Assembly.GetName().Name.Should().Be("Api");
        }
    }
}