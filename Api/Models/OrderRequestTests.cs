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
        public void OrderRequest_DefaultConstructor_ShouldCreateInstance()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
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
        public void OrderRequest_SetProductNameToNull_ShouldAllowNull()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.productname = null;

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderRequest_SetNegativeQuantity_ShouldAllowNegativeValue()
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
        public void OrderStatus_ShouldHaveThreeValues()
        {
            // Act
            var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statusValues.Should().HaveCount(3);
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SetValidStatus_ShouldAcceptAllEnumValues(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = status;

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderStatus_IN_PROGRESS_ShouldBeDefaultValue()
        {
            // Act
            var defaultStatus = default(OrderStatus);

            // Assert
            defaultStatus.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_CompleteObjectInitialization_ShouldSetAllProperties()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 999,
                productname = "Complete Product",
                quantity = 100,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.id.Should().Be(999);
            orderRequest.productname.Should().Be("Complete Product");
            orderRequest.quantity.Should().Be(100);
            orderRequest.status.Should().Be(OrderStatus.REJECTED);
        }
    }
}