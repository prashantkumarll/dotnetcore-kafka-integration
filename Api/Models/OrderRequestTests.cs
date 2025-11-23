using System;
using Xunit;
using FluentAssertions;

namespace Api.Models.Tests
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
            var orderRequest = new OrderRequest
            {
                status = OrderStatus.IN_PROGRESS
            };

            // Act & Assert
            orderRequest.status = OrderStatus.COMPLETED;
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);

            orderRequest.status = OrderStatus.REJECTED;
            orderRequest.status.Should().Be(OrderStatus.REJECTED);
        }

        [Theory]
        [InlineData(0, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(100, "Product2", 0, OrderStatus.COMPLETED)]
        [InlineData(-1, "Product3", 100, OrderStatus.REJECTED)]
        public void OrderRequest_ShouldSupportVariousInputs_Correctly(int id, string productName, int quantity, OrderStatus status)
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
            var expectedStatuses = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Assert
            Enum.GetValues(typeof(OrderStatus)).Should().Contain(expectedStatuses);
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

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                quantity = -5
            };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Fact]
        public void OrderRequest_LargeId_ShouldBeSupported()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = int.MaxValue
            };

            // Assert
            orderRequest.id.Should().Be(int.MaxValue);
        }
    }
}