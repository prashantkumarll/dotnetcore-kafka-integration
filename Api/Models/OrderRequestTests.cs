using System;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest { quantity = -5 };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Fact]
        public void OrderRequest_ProductNameMaxLength_ShouldBeUnlimited()
        {
            // Arrange
            var longProductName = new string('A', 1000);

            // Act
            var orderRequest = new OrderRequest { productname = longProductName };

            // Assert
            orderRequest.productname.Should().Be(longProductName);
        }
    }
}