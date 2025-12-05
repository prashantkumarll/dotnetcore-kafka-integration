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
        public void OrderRequest_ShouldInitializeWithDefaultValues()
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
        public void OrderRequest_ShouldSetAndGetId()
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
        public void OrderRequest_ShouldSetAndGetProductName()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedProductName = "Test Product";

            // Act
            orderRequest.productname = expectedProductName;

            // Assert
            orderRequest.productname.Should().Be(expectedProductName);
        }

        [Fact]
        public void OrderRequest_ShouldSetAndGetQuantity()
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
        public void OrderRequest_ShouldSetAndGetStatus()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedStatus = OrderStatus.COMPLETED;

            // Act
            orderRequest.status = expectedStatus;

            // Assert
            orderRequest.status.Should().Be(expectedStatus);
        }

        [Fact]
        public void OrderRequest_ShouldAcceptAllOrderStatusValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var allStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Act & Assert
            foreach (var status in allStatuses)
            {
                orderRequest.status = status;
                orderRequest.status.Should().Be(status);
            }
        }

        [Fact]
        public void OrderStatus_ShouldHaveThreeValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statusValues.Should().HaveCount(3);
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderRequest_ShouldAllowCompleteObjectInitialization()
        {
            // Arrange
            var expectedId = 456;
            var expectedProductName = "Complete Product";
            var expectedQuantity = 10;
            var expectedStatus = OrderStatus.REJECTED;

            // Act
            var orderRequest = new OrderRequest
            {
                id = expectedId,
                productname = expectedProductName,
                quantity = expectedQuantity,
                status = expectedStatus
            };

            // Assert
            orderRequest.id.Should().Be(expectedId);
            orderRequest.productname.Should().Be(expectedProductName);
            orderRequest.quantity.Should().Be(expectedQuantity);
            orderRequest.status.Should().Be(expectedStatus);
        }
    }
}