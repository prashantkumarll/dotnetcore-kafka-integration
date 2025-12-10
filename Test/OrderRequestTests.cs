using Xunit;
using Api.Models;
using FluentAssertions;
using System;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Should_Be_Instantiable()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_Should_Be_Reference_Type()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsClass.Should().BeTrue();
            type.IsValueType.Should().BeFalse();
        }

        [Fact]
        public void OrderRequest_Should_Have_Default_Constructor()
        {
            // Act & Assert
            var act = () => new OrderRequest();
            act.Should().NotThrow();
        }

        [Fact]
        public void OrderRequest_Multiple_Instances_Should_Be_Different()
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
        public void OrderRequest_Type_Should_Be_In_Correct_Namespace()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.Namespace.Should().Be("Api.Models");
            type.Name.Should().Be("OrderRequest");
        }
    }
}