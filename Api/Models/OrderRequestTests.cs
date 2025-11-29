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
        public void OrderRequest_AllowsNullProductName()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 2,
                productname = null,
                quantity = 5,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SupportAllOrderStatusValues(OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 3,
                productname = "AnyProduct",
                quantity = 7,
                status = status
            };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_CanChangeStatusAfterInitialization()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 4,
                productname = "InitialProduct",
                quantity = 15,
                status = OrderStatus.IN_PROGRESS
            };

            // Act
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void OrderRequest_SupportVariousQuantities(int quantity)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 5,
                productname = "QuantityTest",
                quantity = quantity,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.quantity.Should().Be(quantity);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_InitializesEmptyObject()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(default(OrderStatus));
        }

        [Fact]
        public void OrderStatus_EnumValues_AreCorrectlyDefined()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act & Assert
            Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().Should().BeEquivalentTo(expectedValues);
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var order1 = new OrderRequest { id = 1, productname = "Product1", quantity = 10, status = OrderStatus.IN_PROGRESS };
            var order2 = new OrderRequest { id = 2, productname = "Product2", quantity = 20, status = OrderStatus.COMPLETED };

            // Assert
            order1.id.Should().NotBe(order2.id);
            order1.productname.Should().NotBe(order2.productname);
            order1.quantity.Should().NotBe(order2.quantity);
            order1.status.Should().NotBe(order2.status);
        }
    }
}