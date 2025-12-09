using Xunit;
using FluentAssertions;
using Api.Models;
using System;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldBeInstantiated()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_ShouldBeReferenceType()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsClass.Should().BeTrue();
            type.IsValueType.Should().BeFalse();
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
        public void OrderRequest_ShouldHaveParameterlessConstructor()
        {
            // Arrange & Act
            var constructor = typeof(OrderRequest).GetConstructor(Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
            orderRequest1.Should().NotBeNull();
            orderRequest2.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_ShouldBeAssignableToObject()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().BeAssignableTo<object>();
        }
    }
}