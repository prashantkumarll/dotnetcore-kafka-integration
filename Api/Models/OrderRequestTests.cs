using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_InvalidQuantity_ShouldAllowZeroAndNegativeValues()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest { quantity = 0 };
            var orderRequest2 = new OrderRequest { quantity = -10 };

            // Assert
            orderRequest1.quantity.Should().Be(0);
            orderRequest2.quantity.Should().Be(-10);
        }

        [Fact]
        public void OrderRequest_StatusTransition_ShouldSupportAllDefinedStatuses()
        {
            // Arrange
            var orderRequest = new OrderRequest { status = OrderStatus.IN_PROGRESS };

            // Act & Assert
            orderRequest.status = OrderStatus.COMPLETED;
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);

            orderRequest.status = OrderStatus.REJECTED;
            orderRequest.status.Should().Be(OrderStatus.REJECTED);
        }
    }
}