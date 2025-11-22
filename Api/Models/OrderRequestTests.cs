using System;
using Xunit;
using FluentAssertions;

namespace Api.Models.Tests
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
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(1);
            orderRequest.productname.Should().Be("TestProduct");
            orderRequest.quantity.Should().Be(5);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
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

        [Theory]
        [InlineData(0, "Product", 10, OrderStatus.IN_PROGRESS)]
        [InlineData(100, "AnotherProduct", 0, OrderStatus.COMPLETED)]
        public void OrderRequest_SetProperties_ShouldUpdateCorrectly(int id, string productName, int quantity, OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = id;
            orderRequest.productname = productName;
            orderRequest.quantity = quantity;
            orderRequest.status = status;

            // Assert
            orderRequest.id.Should().Be(id);
            orderRequest.productname.Should().Be(productName);
            orderRequest.quantity.Should().Be(quantity);
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderStatus_AllEnumValues_ShouldBeValid()
        {
            // Arrange
            var enumValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            enumValues.Should().Contain(OrderStatus.IN_PROGRESS);
            enumValues.Should().Contain(OrderStatus.COMPLETED);
            enumValues.Should().Contain(OrderStatus.REJECTED);
            enumValues.Length.Should().Be(3);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderStatus_EnumValues_ShouldBeAssignableToOrderRequest(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = status;

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                quantity = -5
            };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                productname = null
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderRequest_LargeId_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = int.MaxValue
            };

            // Assert
            orderRequest.id.Should().Be(int.MaxValue);
        }
    }
}