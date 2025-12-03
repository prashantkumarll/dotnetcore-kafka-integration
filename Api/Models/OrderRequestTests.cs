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
        [InlineData(1, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 0, OrderStatus.COMPLETED)]
        [InlineData(3, "Product3", 15, OrderStatus.REJECTED)]
        public void OrderRequest_ShouldSupportMultipleStatusValues(int id, string productName, int quantity, OrderStatus status)
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
        public void OrderRequest_CanChangeStatusAfterInitialization()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "InitialProduct",
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            // Act
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void OrderRequest_ShouldAllowNegativeId()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = -1,
                productname = "TestProduct",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(-1);
        }

        [Fact]
        public void OrderRequest_ShouldAllowEmptyProductName()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "",
                quantity = 5,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.productname.Should().Be("");
        }

        [Fact]
        public void OrderRequest_ShouldAllowNegativeQuantity()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = -5,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void OrderRequest_ShouldHandleBoundaryValues_ForId(int boundaryId)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = boundaryId,
                productname = "BoundaryTest",
                quantity = 1,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(boundaryId);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void OrderRequest_ShouldHandleBoundaryValues_ForQuantity(int boundaryQuantity)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "BoundaryTest",
                quantity = boundaryQuantity,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.quantity.Should().Be(boundaryQuantity);
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
        public void OrderStatus_ShouldHaveCorrectEnumOrder()
        {
            // Arrange & Act
            var enumValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            enumValues[0].Should().Be(OrderStatus.IN_PROGRESS);
            enumValues[1].Should().Be(OrderStatus.COMPLETED);
            enumValues[2].Should().Be(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderRequest_ShouldAllowPropertyChaining()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 100,
                productname = "ChainedProduct",
                quantity = 25,
                status = OrderStatus.IN_PROGRESS
            };

            orderRequest.status = OrderStatus.COMPLETED;
            orderRequest.quantity = 30;

            // Assert
            orderRequest.id.Should().Be(100);
            orderRequest.productname.Should().Be("ChainedProduct");
            orderRequest.quantity.Should().Be(30);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}