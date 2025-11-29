using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests.Models
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
        public void OrderRequest_AllStatusValues_ShouldBeValid()
        {
            // Arrange
            var statusValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Theory]
        [InlineData(1, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 0, OrderStatus.COMPLETED)]
        [InlineData(3, "Product3", 100, OrderStatus.REJECTED)]
        public void OrderRequest_MultipleInstances_ShouldSetPropertiesCorrectly(int id, string productName, int quantity, OrderStatus status)
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
        public void OrderRequest_DefaultConstructor_ShouldInitializeDefaultValues()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(default(OrderStatus));
        }

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange & Act
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

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed(int negativeQuantity)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = negativeQuantity,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.quantity.Should().Be(negativeQuantity);
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldHaveExpectedCount()
        {
            // Arrange
            var statusValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statusValues.Length.Should().Be(3);
        }

        [Fact]
        public void OrderRequest_StatusChange_ShouldUpdateCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest { status = OrderStatus.IN_PROGRESS };

            // Act
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}