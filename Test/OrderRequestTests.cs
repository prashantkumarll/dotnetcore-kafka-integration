using Api.Models;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldBeReferenceType()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_ShouldHaveParameterlessConstructor()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_TypeShouldBeClass()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.Should().NotBeNull();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeUnique()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
            orderRequest1.Should().NotBeNull();
            orderRequest2.Should().NotBeNull();
        }
    }
}