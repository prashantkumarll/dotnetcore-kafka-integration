using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ValidData_ShouldCreateInstance()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(1);
            orderRequest.productname.Should().Be("TestProduct");
            orderRequest.quantity.Should().Be(10);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Theory]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        [InlineData(OrderStatus.IN_PROGRESS)]
        public void OrderRequest_AllStatusValues_ShouldBeValid(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = 5,
                status = status
            };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 2,
                productname = null,
                quantity = 0,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 3,
                productname = "NegativeProduct",
                quantity = -5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Fact]
        public void OrderRequest_ZeroId_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 0,
                productname = "ZeroIdProduct",
                quantity = 1,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.id.Should().Be(0);
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldMatchExpected()
        {
            // Assert
            Enum.GetNames(typeof(OrderStatus)).Should().HaveCount(3);
            Enum.GetNames(typeof(OrderStatus)).Should().Contain(new[] { "IN_PROGRESS", "COMPLETED", "REJECTED" });
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldHaveDifferentValues()
        {
            // Arrange
            var orderRequest1 = new OrderRequest
            {
                id = 1,
                productname = "Product1",
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            var orderRequest2 = new OrderRequest
            {
                id = 2,
                productname = "Product2",
                quantity = 20,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
            orderRequest1.id.Should().NotBe(orderRequest2.id);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }
    }
}