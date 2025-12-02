using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests.Models
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Constructor_ShouldInitializePropertiesCorrectly()
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
        public void OrderStatus_ShouldSupportAllDefinedValues(OrderStatus status)
        {
            // Act & Assert
            status.Should().BeOfType<OrderStatus>();
        }

        [Fact]
        public void OrderRequest_SetProperties_ShouldUpdateValuesCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = 42;
            orderRequest.productname = "UpdatedProduct";
            orderRequest.quantity = 25;
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.id.Should().Be(42);
            orderRequest.productname.Should().Be("UpdatedProduct");
            orderRequest.quantity.Should().Be(25);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Theory]
        [InlineData(0, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(100, "Product2", 0, OrderStatus.REJECTED)]
        [InlineData(-1, "Product3", 100, OrderStatus.COMPLETED)]
        public void OrderRequest_MultipleInstances_ShouldCreateIndependentObjects(int id, string productName, int quantity, OrderStatus status)
        {
            // Arrange & Act
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
        public void OrderStatus_EnumValues_ShouldBeConsistent()
        {
            // Arrange
            var statusValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statusValues.Should().HaveLength(3);
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { productname = null };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { quantity = -5 };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }
    }
}