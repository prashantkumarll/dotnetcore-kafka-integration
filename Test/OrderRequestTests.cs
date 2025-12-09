using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldBeReferenceType()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_ShouldBeInstantiable()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.GetType().Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_ShouldHaveCorrectClassName()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.GetType().Name.Should().Be("OrderRequest");
        }

        [Fact]
        public void OrderRequest_ShouldNotBeAbstract()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void OrderRequest_ShouldNotBeInterface()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsInterface.Should().BeFalse();
        }
    }
}