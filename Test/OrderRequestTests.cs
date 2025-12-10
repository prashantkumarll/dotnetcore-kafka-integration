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
            // Act
            var constructor = typeof(OrderRequest).GetConstructor(Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_Should_Be_In_Models_Namespace()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.GetType().Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_Type_Should_Be_Public()
        {
            // Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsPublic.Should().BeTrue();
        }
    }
}