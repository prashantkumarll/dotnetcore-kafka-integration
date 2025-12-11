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
        public void OrderRequest_ShouldAllowStatusChanges_BetweenValidStatuses()
        {
            // Arrange
            var orderRequest = new OrderRequest { status = OrderStatus.IN_PROGRESS };

            // Act
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void OrderStatus_ShouldContainExpectedValues()
        {
            // Arrange
            var expectedStatuses = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Assert
            Enum.GetValues(typeof(OrderStatus)).Should().Contain(expectedStatuses);
        }

        [Theory]
        [InlineData(1, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 0, OrderStatus.REJECTED)]
        [InlineData(3, "Product3", 100, OrderStatus.COMPLETED)]
        public void OrderRequest_ShouldCreateInstanceWithDifferentValues(int id, string productName, int quantity, OrderStatus status)
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
        public void OrderRequest_DefaultConstructor_ShouldInitializePropertiesToDefault()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest { quantity = -5 };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Fact]
        public void OrderRequest_LargeQuantity_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest { quantity = 1000000 };

            // Assert
            orderRequest.quantity.Should().Be(1000000);
        }

        [Fact]
        public void OrderRequest_EmptyProductName_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest { productname = string.Empty };

            // Assert
            orderRequest.productname.Should().BeEmpty();
        }
    }
}