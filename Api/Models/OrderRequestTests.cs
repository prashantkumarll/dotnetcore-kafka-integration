using System;
using System.Linq;
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
            // Arrange & Act
            var enumValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            enumValues.Should().Contain(OrderStatus.IN_PROGRESS);
            enumValues.Should().Contain(OrderStatus.COMPLETED);
            enumValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_ShouldAllowAllOrderStatusValues(OrderStatus status)
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

        [Theory]
        [InlineData(0, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(100, "Product2", 15, OrderStatus.COMPLETED)]
        [InlineData(999, "Product3", 25, OrderStatus.REJECTED)]
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
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = -5,
                status = OrderStatus.IN_PROGRESS
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
                id = 1,
                productname = null,
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderRequest_ZeroId_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 0,
                productname = "TestProduct",
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(0);
        }

        [Fact]
        public void OrderStatus_ShouldHaveExactlyThreeValues()
        {
            // Arrange & Act
            var enumValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            enumValues.Should().HaveCount(3);
        }

        [Fact]
        public void OrderRequest_EmptyProductName_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = string.Empty,
                quantity = 10,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.productname.Should().Be(string.Empty);
        }

        [Fact]
        public void OrderRequest_MaxIntValues_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = int.MaxValue,
                productname = "MaxProduct",
                quantity = int.MaxValue,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.id.Should().Be(int.MaxValue);
            orderRequest.quantity.Should().Be(int.MaxValue);
        }

        [Fact]
        public void OrderRequest_MinIntValues_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = int.MinValue,
                productname = "MinProduct",
                quantity = int.MinValue,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(int.MinValue);
            orderRequest.quantity.Should().Be(int.MinValue);
        }

        [Theory]
        [InlineData("Product with spaces")]
        [InlineData("Product123")]
        [InlineData("UPPERCASE")]
        [InlineData("lowercase")]
        [InlineData("Special-Characters_123")]
        public void OrderRequest_VariousProductNames_ShouldBeAllowed(string productName)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = productName,
                quantity = 5,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.productname.Should().Be(productName);
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldHaveCorrectUnderlyingValues()
        {
            // Arrange & Act & Assert
            ((int)OrderStatus.IN_PROGRESS).Should().Be(0);
            ((int)OrderStatus.COMPLETED).Should().Be(1);
            ((int)OrderStatus.REJECTED).Should().Be(2);
        }

        [Fact]
        public void OrderRequest_PropertyModification_ShouldUpdateCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "InitialProduct",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Act
            orderRequest.id = 999;
            orderRequest.productname = "UpdatedProduct";
            orderRequest.quantity = 100;
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.id.Should().Be(999);
            orderRequest.productname.Should().Be("UpdatedProduct");
            orderRequest.quantity.Should().Be(100);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}