using Xunit;
using Api.Models;
using FluentAssertions;
using System;

namespace Api.Tests.Models
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldBeReferenceType()
        {
            // Arrange & Act
            var orderRequestType = typeof(OrderRequest);

            // Assert
            orderRequestType.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_ShouldBeInstantiable()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var orderRequestType = typeof(OrderRequest);

            // Assert
            orderRequestType.Namespace.Should().Be("Api.Models");
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
        public void OrderRequest_MultipleInstances_ShouldBeUnique()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
        }
    }
}