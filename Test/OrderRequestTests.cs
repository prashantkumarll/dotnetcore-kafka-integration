using Xunit;
using FluentAssertions;
using Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_WithValidData_ShouldPass()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-12345",
                ProductName = "Test Product",
                Quantity = 5,
                Price = 199.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void OrderRequest_WithInvalidOrderId_ShouldFail(string? orderId)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = orderId!,
                ProductName = "Test Product",
                Quantity = 1,
                Price = 99.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("OrderId"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void OrderRequest_WithInvalidProductName_ShouldFail(string? productName)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-123",
                ProductName = productName!,
                Quantity = 1,
                Price = 99.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("ProductName"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void OrderRequest_WithInvalidQuantity_ShouldFail(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-123",
                ProductName = "Test Product",
                Quantity = quantity,
                Price = 99.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Quantity"));
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-100.00)]
        public void OrderRequest_WithNegativePrice_ShouldFail(double price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-123",
                ProductName = "Test Product",
                Quantity = 1,
                Price = (decimal)price
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Price"));
        }

        [Fact]
        public void OrderRequest_WithZeroPrice_ShouldPass()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-123",
                ProductName = "Free Sample",
                Quantity = 1,
                Price = 0m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void OrderRequest_Properties_ShouldBeSettableAndGettable()
        {
            // Arrange
            var orderId = "TEST-001";
            var productName = "Test Product";
            var quantity = 3;
            var price = 150.75m;

            // Act
            var orderRequest = new OrderRequest
            {
                OrderId = orderId,
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };

            // Assert
            orderRequest.OrderId.Should().Be(orderId);
            orderRequest.ProductName.Should().Be(productName);
            orderRequest.Quantity.Should().Be(quantity);
            orderRequest.Price.Should().Be(price);
        }

        private static List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}