using System;
using Xunit;
using FluentAssertions;

namespace Api.Models.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_DefaultConstructor_PropertiesInitialized()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_SetProperties_ValuesCorrectlyAssigned()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = 10,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.id.Should().Be(1);
            orderRequest.productname.Should().Be("TestProduct");
            orderRequest.quantity.Should().Be(10);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderStatus_ValidValues_ShouldBeAccepted(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest { status = status };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_NullProductName_Allowed()
        {
            // Arrange
            var orderRequest = new OrderRequest { productname = null };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void OrderRequest_QuantityValues_ShouldBeAccepted(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest { quantity = quantity };

            // Assert
            orderRequest.quantity.Should().Be(quantity);
        }

        [Fact]
        public void OrderRequest_NegativeId_Allowed()
        {
            // Arrange
            var orderRequest = new OrderRequest { id = -1 };

            // Assert
            orderRequest.id.Should().Be(-1);
        }

        [Fact]
        public void OrderRequest_MultipleInstances_IndependentProperties()
        {
            // Arrange
            var orderRequest1 = new OrderRequest { id = 1, productname = "Product1" };
            var orderRequest2 = new OrderRequest { id = 2, productname = "Product2" };

            // Assert
            orderRequest1.id.Should().Be(1);
            orderRequest1.productname.Should().Be("Product1");
            orderRequest2.id.Should().Be(2);
            orderRequest2.productname.Should().Be("Product2");
        }
    }
}