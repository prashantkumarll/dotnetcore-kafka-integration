using Api.Models;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Should_Be_Instantiated()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_Should_Be_Reference_Type()
        {
            // Arrange & Act
            var orderRequestType = typeof(OrderRequest);

            // Assert
            orderRequestType.Should().NotBeNull();
            orderRequestType.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_Should_Have_Parameterless_Constructor()
        {
            // Arrange & Act
            var constructor = typeof(OrderRequest).GetConstructor(System.Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_Multiple_Instances_Should_Be_Different()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeNull();
            orderRequest2.Should().NotBeNull();
            orderRequest1.Should().NotBeSameAs(orderRequest2);
        }

        [Fact]
        public void OrderRequest_Should_Be_In_Correct_Namespace()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();
            var namespaceName = orderRequest.GetType().Namespace;

            // Assert
            namespaceName.Should().Be("Api.Models");
        }
    }
}