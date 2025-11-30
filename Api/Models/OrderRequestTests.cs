using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Constructor_ShouldInitializeProperties()
        {
            // Arrange & Act
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

        [Fact]
        public void OrderRequest_AllowsNullProductName()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 2,
                productname = null,
                quantity = 5,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SupportAllOrderStatusValues(OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 3,
                productname = "AnyProduct",
                quantity = 7,
                status = status
            };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_CanChangeProperties()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = 4;
            orderRequest.productname = "UpdatedProduct";
            orderRequest.quantity = 15;
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.id.Should().Be(4);
            orderRequest.productname.Should().Be("UpdatedProduct");
            orderRequest.quantity.Should().Be(15);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void OrderStatus_EnumValues_AreCorrect()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
            statusValues.Length.Should().Be(3);
        }

        [Theory]
        [InlineData(0, "TestProduct", 10, OrderStatus.IN_PROGRESS)]
        [InlineData(1, "AnotherProduct", 5, OrderStatus.COMPLETED)]
        [InlineData(2, "LastProduct", 20, OrderStatus.REJECTED)]
        public void OrderRequest_MultipleInstances_ShouldHaveDifferentValues(int id, string productName, int quantity, OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = id,
                productname = productName,
                quantity = quantity,
                status = status
            };

            // Assert
            orderRequest.id.Should().Be(id);
            orderRequest.productname.Should().Be(productName);
            orderRequest.quantity.Should().Be(quantity);
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldCreateEmptyInstance()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
        }

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 5,
                productname = "NegativeQuantityProduct",
                quantity = -10,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.quantity.Should().Be(-10);
        }
    }
}