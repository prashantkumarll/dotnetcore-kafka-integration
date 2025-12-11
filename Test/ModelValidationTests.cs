using Xunit;
using FluentAssertions;
using Api.Models;
using System;

namespace Test
{
    public class ModelValidationTests
    {
        [Theory]
        [InlineData("ORD-001", "John Doe", 100.50)]
        [InlineData("ORD-999", "Jane Smith", 0.01)]
        [InlineData("ORD-123", "Test Customer", 999999.99)]
        public void OrderRequest_WithValidData_ShouldSetPropertiesCorrectly(string orderId, string customerName, decimal amount)
        {
            // Arrange & Act
            var order = new OrderRequest
            {
                OrderId = orderId,
                CustomerName = customerName,
                Amount = amount
            };

            // Assert
            order.OrderId.Should().Be(orderId);
            order.CustomerName.Should().Be(customerName);
            order.Amount.Should().Be(amount);
        }

        [Fact]
        public void OrderRequest_WithLongOrderId_ShouldAcceptLongString()
        {
            // Arrange
            var longOrderId = new string('A', 1000);
            var order = new OrderRequest();

            // Act
            order.OrderId = longOrderId;

            // Assert
            order.OrderId.Should().Be(longOrderId);
            order.OrderId.Length.Should().Be(1000);
        }

        [Fact]
        public void OrderRequest_WithSpecialCharacters_ShouldAcceptSpecialCharacters()
        {
            // Arrange
            var specialOrderId = "ORD-!@#$%^&*()_+-=[]{}|;':\",./<>?";
            var specialCustomerName = "Customer áéíóú ñüçß 中文 🚀";

            // Act
            var order = new OrderRequest
            {
                OrderId = specialOrderId,
                CustomerName = specialCustomerName
            };

            // Assert
            order.OrderId.Should().Be(specialOrderId);
            order.CustomerName.Should().Be(specialCustomerName);
        }

        [Theory]
        [InlineData(-100.50)]
        [InlineData(-0.01)]
        [InlineData(decimal.MinValue)]
        [InlineData(decimal.MaxValue)]
        public void OrderRequest_WithEdgeCaseAmounts_ShouldAcceptAllDecimalValues(decimal amount)
        {
            // Arrange & Act
            var order = new OrderRequest
            {
                Amount = amount
            };

            // Assert
            order.Amount.Should().Be(amount);
        }

        [Fact]
        public void OrderRequest_PropertyAssignment_ShouldBeIndependent()
        {
            // Arrange
            var order1 = new OrderRequest { OrderId = "ORD-1" };
            var order2 = new OrderRequest { OrderId = "ORD-2" };

            // Act & Assert
            order1.OrderId.Should().Be("ORD-1");
            order2.OrderId.Should().Be("ORD-2");
            order1.OrderId.Should().NotBe(order2.OrderId);
        }
    }
}