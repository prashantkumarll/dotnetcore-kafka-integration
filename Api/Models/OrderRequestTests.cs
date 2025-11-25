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
        public void OrderStatus_ShouldSupportAllDefinedValues(OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { status = status };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_CanChangeProperties()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = 3;
            orderRequest.productname = "UpdatedProduct";
            orderRequest.quantity = 15;
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.id.Should().Be(3);
            orderRequest.productname.Should().Be("UpdatedProduct");
            orderRequest.quantity.Should().Be(15);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void OrderRequest_QuantityCanBeSetToValidValues(int quantity)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { quantity = quantity };

            // Assert
            orderRequest.quantity.Should().Be(quantity);
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

        [Fact]
        public void OrderStatus_EnumValuesAreCorrect()
        {
            // Arrange
            var enumValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            enumValues.Should().Contain(OrderStatus.IN_PROGRESS);
            enumValues.Should().Contain(OrderStatus.COMPLETED);
            enumValues.Should().Contain(OrderStatus.REJECTED);
            enumValues.Length.Should().Be(3);
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldWorkIndependently()
        {
            // Arrange
            var order1 = new OrderRequest { id = 1, productname = "Product1", quantity = 10, status = OrderStatus.IN_PROGRESS };
            var order2 = new OrderRequest { id = 2, productname = "Product2", quantity = 20, status = OrderStatus.COMPLETED };

            // Assert
            order1.id.Should().Be(1);
            order1.productname.Should().Be("Product1");
            order1.quantity.Should().Be(10);
            order1.status.Should().Be(OrderStatus.IN_PROGRESS);

            order2.id.Should().Be(2);
            order2.productname.Should().Be("Product2");
            order2.quantity.Should().Be(20);
            order2.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}