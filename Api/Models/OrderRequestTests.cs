using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldCreateValidInstance_WithAllProperties()
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

        [Fact]
        public void OrderStatus_ShouldHaveCorrectEnumValues()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Assert
            Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().Should().BeEquivalentTo(expectedValues);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_ShouldSupportAllOrderStatusValues(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = 10,
                status = status
            };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldInitializePropertiesToDefaultValues()
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
        [InlineData(0, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(100, "Product2", 20, OrderStatus.COMPLETED)]
        [InlineData(999, "Product3", 0, OrderStatus.REJECTED)]
        public void OrderRequest_ShouldAllowVariousPropertyConfigurations(int id, string productName, int quantity, OrderStatus status)
        {
            // Arrange
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
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = null,
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }
    }
}