using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Models.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_MaximumQuantity_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "LargeOrder",
                quantity = int.MaxValue,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.quantity.Should().Be(int.MaxValue);
        }

        [Fact]
        public void OrderRequest_MinimumQuantity_ShouldBeAllowed()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "SmallOrder",
                quantity = int.MinValue,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.quantity.Should().Be(int.MinValue);
        }
    }
}