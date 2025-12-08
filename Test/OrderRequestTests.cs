using Api.Models;
using FluentAssertions;
using System;
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
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_ShouldBeReferenceType()
        {
            // Act & Assert
            typeof(OrderRequest).Should().NotBeValueType();
            typeof(OrderRequest).IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_ShouldHaveParameterlessConstructor()
        {
            // Act
            var constructor = typeof(OrderRequest).GetConstructor(Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeDifferentObjects()
        {
            // Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
            ReferenceEquals(orderRequest1, orderRequest2).Should().BeFalse();
        }

        [Fact]
        public void OrderRequest_ShouldBeInCorrectNamespace()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.GetType().Namespace.Should().Be("Api.Models");
            orderRequest.GetType().FullName.Should().Be("Api.Models.OrderRequest");
        }
    }
}