using System;
using Xunit;
using FluentAssertions;

namespace Api.Models.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_CreateWithValidData_ShouldInitializeCorrectly()
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
        public void OrderRequest_ChangeStatus_ShouldUpdateCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            // Act
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Theory]
        [InlineData(0, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 0, OrderStatus.REJECTED)]
        public void OrderRequest_CreateWithDifferentData_ShouldInitializeCorrectly(int id, string productName, int quantity, OrderStatus status)
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
        public void OrderRequest_DefaultConstructor_ShouldCreateEmptyObject()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
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
                id = 1,
                productname = null,
                quantity = 10,
                status = OrderStatus.COMPLETED
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
                id = 1,
                productname = "TestProduct",
                quantity = -5,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Fact]
        public void OrderStatus_EnumValuesUnchanged()
        {
            // Arrange
            var statuses = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statuses.Should().HaveCount(3);
            statuses.Should().Contain(OrderStatus.IN_PROGRESS);
            statuses.Should().Contain(OrderStatus.COMPLETED);
            statuses.Should().Contain(OrderStatus.REJECTED);
        }
    }
}