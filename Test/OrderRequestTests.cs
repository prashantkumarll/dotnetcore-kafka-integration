using Xunit;
using FluentAssertions;
using Api.Models;
using System;

namespace Api.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldBeInstantiated()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_ShouldBeReferenceType()
        {
            // Act & Assert
            typeof(OrderRequest).IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act & Assert
            orderRequest.GetType().Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_ShouldBeOfCorrectType()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act & Assert
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_ShouldAllowMultipleInstances()
        {
            // Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeNull();
            orderRequest2.Should().NotBeNull();
            orderRequest1.Should().NotBeSameAs(orderRequest2);
        }

        [Fact]
        public void OrderRequest_ShouldHaveParameterlessConstructor()
        {
            // Act & Assert
            Action act = () => new OrderRequest();
            act.Should().NotThrow();
        }

        [Fact]
        public void OrderRequest_TypeShouldNotBeNull()
        {
            // Act & Assert
            typeof(OrderRequest).Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_ShouldBeConcreteClass()
        {
            // Act & Assert
            typeof(OrderRequest).IsAbstract.Should().BeFalse();
            typeof(OrderRequest).IsInterface.Should().BeFalse();
        }
    }
}