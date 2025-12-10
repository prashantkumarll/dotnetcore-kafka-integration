using Api.Models;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldInstantiateSuccessfully()
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
        public void OrderRequest_ShouldBeInCorrectNamespace()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_ShouldHaveParameterlessConstructor()
        {
            // Act
            var constructors = typeof(OrderRequest).GetConstructors();
            var parameterlessConstructor = constructors.Where(c => c.GetParameters().Length == 0);

            // Assert
            parameterlessConstructor.Should().NotBeEmpty();
        }

        [Fact]
        public void OrderRequest_ShouldCreateMultipleInstances()
        {
            // Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeNull();
            orderRequest2.Should().NotBeNull();
            orderRequest1.Should().NotBeSameAs(orderRequest2);
        }
    }
}