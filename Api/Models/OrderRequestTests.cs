using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ValidData_ShouldCreateInstance()
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
        public void OrderRequest_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Theory]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SetStatus_ShouldUpdateStatusCorrectly(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = status;

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                productname = null
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-1)]
        public void OrderRequest_SetQuantity_ShouldUpdateQuantityCorrectly(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = quantity;

            // Assert
            orderRequest.quantity.Should().Be(quantity);
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldBeCorrect()
        {
            // Assert
            Enum.GetValues(typeof(OrderStatus)).Should().HaveCount(3);
            Enum.IsDefined(typeof(OrderStatus), OrderStatus.IN_PROGRESS).Should().BeTrue();
            Enum.IsDefined(typeof(OrderStatus), OrderStatus.COMPLETED).Should().BeTrue();
            Enum.IsDefined(typeof(OrderStatus), OrderStatus.REJECTED).Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_SetId_ShouldUpdateIdCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = 42;

            // Assert
            orderRequest.id.Should().Be(42);
        }

        [Fact]
        public void OrderRequest_SetProductName_ShouldUpdateProductNameCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.productname = "NewProduct";

            // Assert
            orderRequest.productname.Should().Be("NewProduct");
        }
    }
}