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
        public void OrderRequest_ShouldSupportAllOrderStatusValues()
        {
            // Arrange & Act
            var statuses = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statuses.Should().Contain(OrderStatus.IN_PROGRESS);
            statuses.Should().Contain(OrderStatus.COMPLETED);
            statuses.Should().Contain(OrderStatus.REJECTED);
        }

        [Theory]
        [InlineData(0, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 15, OrderStatus.COMPLETED)]
        [InlineData(3, "Product3", 0, OrderStatus.REJECTED)]
        public void OrderRequest_ShouldSetPropertiesCorrectly(int id, string productName, int quantity, OrderStatus status)
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
        public void OrderStatus_ShouldHaveExpectedValues()
        {
            // Arrange
            var statusValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statusValues.Length.Should().Be(3);
            statusValues.Should().Contain(new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED });
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
        public void OrderRequest_CanChangeProperties_AfterInitialization()
        {
            // Arrange
            var orderRequest = new OrderRequest { id = 1, productname = "Initial" };

            // Act
            orderRequest.id = 2;
            orderRequest.productname = "Updated";
            orderRequest.quantity = 20;
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.id.Should().Be(2);
            orderRequest.productname.Should().Be("Updated");
            orderRequest.quantity.Should().Be(20);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderStatus_ShouldSupportAllDefinedValues(OrderStatus status)
        {
            // Assert
            status.Should().BeOfType<OrderStatus>();
        }
    }
}