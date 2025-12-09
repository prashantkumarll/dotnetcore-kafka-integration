using Api.Models;
using Xunit;
using FluentAssertions;

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
            type.IsValueType.Should().BeFalse();
        }

        [Fact]
        public void OrderRequest_ShouldHaveCorrectNamespace()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_ShouldHaveCorrectTypeName()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.Name.Should().Be("OrderRequest");
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeUnique()
        {
            // Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
        }
    }
}