using System;
using Xunit;
using FluentAssertions;

namespace Api.Models.Tests
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
        public void OrderRequest_ShouldAllowStatusChanges()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act & Assert
            orderRequest.status = OrderStatus.COMPLETED;
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);

            orderRequest.status = OrderStatus.REJECTED;
            orderRequest.status.Should().Be(OrderStatus.REJECTED);
        }

        [Theory]
        [InlineData(1, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 0, OrderStatus.COMPLETED)]
        [InlineData(3, "Product3", 100, OrderStatus.REJECTED)]
        public void OrderRequest_ShouldSupportVariousInputs(int id, string productName, int quantity, OrderStatus status)
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
        public void OrderStatus_ShouldHaveExpectedValues()
        {
            // Arrange
            var expectedStatuses = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act
            var actualStatuses = Enum.GetValues(typeof(OrderStatus));

            // Assert
            actualStatuses.Should().Contain(expectedStatuses);
            actualStatuses.Length.Should().Be(3);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed(int negativeQuantity)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                quantity = negativeQuantity
            };

            // Assert
            orderRequest.quantity.Should().Be(negativeQuantity);
        }

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                productname = null
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderRequest_MultipleStatusChanges_ShouldWork()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act & Assert
            orderRequest.status = OrderStatus.IN_PROGRESS;
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);

            orderRequest.status = OrderStatus.COMPLETED;
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);

            orderRequest.status = OrderStatus.REJECTED;
            orderRequest.status.Should().Be(OrderStatus.REJECTED);
        }
    }
}