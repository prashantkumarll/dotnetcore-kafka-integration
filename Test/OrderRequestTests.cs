using Api.Models;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldBeInstantiable()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_ShouldBeReferenceType()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_ShouldHaveDefaultConstructor()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_ShouldBeInModelsNamespace()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_ShouldNotBeAbstract()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsAbstract.Should().BeFalse();
        }
    }
}