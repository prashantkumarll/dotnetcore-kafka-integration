using System;
using System.Linq;
using Api.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.Models
{
    public class OrderRequestTests
    {
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
        public void OrderRequest_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 123;
            var expectedProductName = "Test Product";
            var expectedQuantity = 5;
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

        [Fact]
        public void OrderRequest_SetNullProductName_ShouldAcceptNull()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.productname = null;

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderRequest_SetNegativeQuantity_ShouldAcceptNegativeValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var negativeQuantity = -10;

            // Act
            orderRequest.quantity = negativeQuantity;

            // Assert
            orderRequest.quantity.Should().Be(negativeQuantity);
        }

        [Fact]
        public void OrderStatus_Enum_ShouldContainAllExpectedValues()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act
            var actualValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            actualValues.Should().HaveCount(3);
            actualValues.Should().BeEquivalentTo(expectedValues);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SetValidOrderStatus_ShouldAcceptAllEnumValues(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = status;

            // Assert
            orderRequest.status.Should().Be(status);
        }
    }
}