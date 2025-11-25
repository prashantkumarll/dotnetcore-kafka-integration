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
        public void OrderStatus_ShouldSupportAllDefinedStatuses(OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { status = status };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_CanChangeStatus()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 3,
                productname = "ChangeStatusProduct",
                quantity = 7,
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
        public void OrderRequest_QuantityCanBeSet(int quantity)
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
            var statusValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
            statusValues.Length.Should().Be(3);
        }

        [Fact]
        public void OrderRequest_IdCanBeSet()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { id = 42 };

            // Assert
            orderRequest.id.Should().Be(42);
        }
    }
}