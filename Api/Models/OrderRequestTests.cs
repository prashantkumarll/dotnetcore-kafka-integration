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
        public void OrderRequest_DefaultConstructor_ShouldCreateEmptyInstance()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_StatusEnum_ShouldSupportAllValues(OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { status = status };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { quantity = -5 };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Fact]
        public void OrderRequest_ZeroQuantity_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { quantity = 0 };

            // Assert
            orderRequest.quantity.Should().Be(0);
        }

        [Fact]
        public void OrderRequest_EmptyProductName_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { productname = string.Empty };

            // Assert
            orderRequest.productname.Should().BeEmpty();
        }

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { productname = null! };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldMatchExpectedCount()
        {
            // Arrange
            var enumValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            enumValues.Length.Should().Be(3);
        }
    }
}