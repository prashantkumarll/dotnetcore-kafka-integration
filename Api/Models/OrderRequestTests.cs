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
        public void OrderRequest_DefaultConstructor_ShouldCreateEmptyObject()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Theory]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        [InlineData(OrderStatus.IN_PROGRESS)]
        public void OrderRequest_StatusSetting_ShouldAllowAllEnumValues(OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { status = status };

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_ProductNameCanBeSet_ShouldUpdateCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.productname = "NewProduct";

            // Assert
            orderRequest.productname.Should().Be("NewProduct");
        }

        [Fact]
        public void OrderRequest_QuantityCanBeSet_ShouldUpdateCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = 25;

            // Assert
            orderRequest.quantity.Should().Be(25);
        }

        [Fact]
        public void OrderRequest_IdCanBeSet_ShouldUpdateCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = 42;

            // Assert
            orderRequest.id.Should().Be(42);
        }

        [Fact]
        public void OrderStatus_ShouldHaveExpectedValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderRequest_MultiplePropertiesSet_ShouldMaintainValues()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 100,
                productname = "ComplexProduct",
                quantity = 50,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.id.Should().Be(100);
            orderRequest.productname.Should().Be("ComplexProduct");
            orderRequest.quantity.Should().Be(50);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}