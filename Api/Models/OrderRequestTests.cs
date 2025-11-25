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

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderStatus_ShouldSupportAllDefinedStatuses(OrderStatus status)
        {
            // Act & Assert
            status.Should().BeOfType<OrderStatus>();
        }

        [Fact]
        public void OrderRequest_ShouldAllowZeroQuantity_WhenValid()
        {
            // Arrange
            var orderRequest = new OrderRequest { quantity = 0 };

            // Assert
            orderRequest.quantity.Should().Be(0);
        }

        [Fact]
        public void OrderRequest_ShouldAllowEmptyProductName_WhenValid()
        {
            // Arrange
            var orderRequest = new OrderRequest { productname = "" };

            // Assert
            orderRequest.productname.Should().BeEmpty();
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
            orderRequest.status.Should().Be(default(OrderStatus));
        }

        [Fact]
        public void OrderStatus_ShouldHaveCorrectEnumValues()
        {
            // Arrange
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statuses.Should().Contain(OrderStatus.IN_PROGRESS);
            statuses.Should().Contain(OrderStatus.COMPLETED);
            statuses.Should().Contain(OrderStatus.REJECTED);
            statuses.Length.Should().Be(3);
        }

        [Fact]
        public void OrderRequest_ShouldAllowNegativeId_WhenValid()
        {
            // Arrange
            var orderRequest = new OrderRequest { id = -1 };

            // Assert
            orderRequest.id.Should().Be(-1);
        }
    }
}