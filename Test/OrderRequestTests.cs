using Xunit;
using FluentAssertions;
using Api.Models;
using System;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldBeInstantiable()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
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
        public void OrderRequest_ShouldHavePublicConstructor()
        {
            // Arrange & Act
            var constructors = typeof(OrderRequest).GetConstructors();

            // Assert
            constructors.Should().NotBeEmpty();
            constructors.Should().Contain(c => c.IsPublic);
        }

        [Fact]
        public void OrderRequest_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var namespaceName = orderRequest.GetType().Namespace;

            // Assert
            namespaceName.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeDifferentObjects()
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