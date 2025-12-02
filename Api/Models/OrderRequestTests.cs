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
        [InlineData(1, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 0, OrderStatus.COMPLETED)]
        [InlineData(3, "Product3", 100, OrderStatus.REJECTED)]
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
        public void OrderRequest_DefaultConstructor_ShouldInitializeEmptyInstance()
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
        public void OrderRequest_LongProductName_ShouldBeAllowed()
        {
            // Arrange
            var longProductName = new string('A', 1000);
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = longProductName,
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.productname.Should().Be(longProductName);
        }

        [Fact]
        public void OrderRequest_StatusTransition_ShouldBeAllowed()
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

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldHaveIndependentProperties()
        {
            // Arrange
            var orderRequest1 = new OrderRequest
            {
                id = 1,
                productname = "Product1",
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            var orderRequest2 = new OrderRequest
            {
                id = 2,
                productname = "Product2",
                quantity = 20,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest1.id.Should().Be(1);
            orderRequest1.productname.Should().Be("Product1");
            orderRequest1.quantity.Should().Be(10);
            orderRequest1.status.Should().Be(OrderStatus.IN_PROGRESS);

            orderRequest2.id.Should().Be(2);
            orderRequest2.productname.Should().Be("Product2");
            orderRequest2.quantity.Should().Be(20);
            orderRequest2.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}