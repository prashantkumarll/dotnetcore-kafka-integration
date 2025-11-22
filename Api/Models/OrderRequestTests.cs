using System;
using Xunit;
using FluentAssertions;

namespace Api.Models.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_DefaultConstructor_InitializesPropertiesCorrectly()
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
        public void OrderRequest_SetProperties_ShouldUpdateValuesCorrectly()
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
        public void OrderStatus_AllEnumValues_ShouldBeValid(OrderStatus status)
        {
            // Act & Assert
            status.Should().BeOfType<OrderStatus>();
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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void OrderRequest_QuantityValues_ShouldBeSet(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                quantity = quantity
            };

            // Assert
            orderRequest.quantity.Should().Be(quantity);
        }

        [Fact]
        public void OrderRequest_DefaultStatus_ShouldBeInProgress()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }
    }
}