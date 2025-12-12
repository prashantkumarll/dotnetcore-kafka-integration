using Xunit;
using FluentAssertions;
using Api.Models;
using System;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void Id_SetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 123;

            // Act
            orderRequest.id = expectedId;

            // Assert
            orderRequest.id.Should().Be(expectedId);
        }

        [Fact]
        public void ProductName_SetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedProductName = "TestProduct";

            // Act
            orderRequest.productname = expectedProductName;

            // Assert
            orderRequest.productname.Should().Be(expectedProductName);
        }

        [Fact]
        public void Quantity_SetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedQuantity = 5;

            // Act
            orderRequest.quantity = expectedQuantity;

            // Assert
            orderRequest.quantity.Should().Be(expectedQuantity);
        }

        [Fact]
        public void Status_SetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedStatus = OrderStatus.IN_PROGRESS;

            // Act
            orderRequest.status = expectedStatus;

            // Assert
            orderRequest.status.Should().Be(expectedStatus);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Id_SetNegativeOrZeroValue_ShouldAcceptValue(int id)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = id;

            // Assert
            orderRequest.id.Should().Be(id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Quantity_SetVariousValues_ShouldAcceptValue(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = quantity;

            // Assert
            orderRequest.quantity.Should().Be(quantity);
        }

        [Fact]
        public void OrderRequest_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_SetAllProperties_ShouldRetainValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 456;
            var expectedProductName = "CompleteProduct";
            var expectedQuantity = 10;
            var expectedStatus = OrderStatus.COMPLETED;

            // Act
            orderRequest.id = expectedId;
            orderRequest.productname = expectedProductName;
            orderRequest.quantity = expectedQuantity;
            orderRequest.status = expectedStatus;

            // Assert
            orderRequest.id.Should().Be(expectedId);
            orderRequest.productname.Should().Be(expectedProductName);
            orderRequest.quantity.Should().Be(expectedQuantity);
            orderRequest.status.Should().Be(expectedStatus);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void Status_SetAllValidEnumValues_ShouldAcceptValue(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = status;

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldContainAllExpectedValues()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act
            var actualValues = (OrderStatus[])Enum.GetValues(typeof(OrderStatus));

            // Assert
            actualValues.Should().HaveCount(3);
            actualValues.Should().Contain(OrderStatus.IN_PROGRESS);
            actualValues.Should().Contain(OrderStatus.COMPLETED);
            actualValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Fact]
        public void ProductName_SetEmptyString_ShouldAcceptValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var emptyProductName = string.Empty;

            // Act
            orderRequest.productname = emptyProductName;

            // Assert
            orderRequest.productname.Should().Be(emptyProductName);
        }
    }
}