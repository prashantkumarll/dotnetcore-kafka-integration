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
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
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

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { productname = null };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void OrderRequest_QuantityValues_ShouldBeSetCorrectly(int quantity)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { quantity = quantity };

            // Assert
            orderRequest.quantity.Should().Be(quantity);
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldBeCorrectlyDefined()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act & Assert
            Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().Should().BeEquivalentTo(expectedValues);
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldHaveIndependentState()
        {
            // Arrange
            var request1 = new OrderRequest { id = 1, productname = "Product1", quantity = 10, status = OrderStatus.IN_PROGRESS };
            var request2 = new OrderRequest { id = 2, productname = "Product2", quantity = 20, status = OrderStatus.COMPLETED };

            // Assert
            request1.id.Should().NotBe(request2.id);
            request1.productname.Should().NotBe(request2.productname);
            request1.quantity.Should().NotBe(request2.quantity);
            request1.status.Should().NotBe(request2.status);
        }
    }
}