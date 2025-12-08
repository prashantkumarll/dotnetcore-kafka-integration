using Xunit;
using FluentAssertions;
using Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Test
{
    public class OrderRequestModelTests
    {
        [Fact]
        public void OrderRequest_WithValidData_PassesValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 5,
                Price = 99.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void OrderRequest_WithInvalidCustomerId_FailsValidation(string customerId)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = customerId,
                ProductId = "PROD001",
                Quantity = 1,
                Price = 10.00m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void OrderRequest_WithInvalidProductId_FailsValidation(string productId)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = productId,
                Quantity = 1,
                Price = 10.00m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void OrderRequest_WithInvalidQuantity_FailsValidation(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = quantity,
                Price = 10.00m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-1.00)]
        [InlineData(-100.50)]
        public void OrderRequest_WithNegativePrice_FailsValidation(decimal price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 1,
                Price = price
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
        }

        [Fact]
        public void OrderRequest_WithZeroPrice_PassesValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 1,
                Price = 0.00m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void OrderRequest_WithLargeValues_PassesValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = "CUST999999",
                ProductId = "PROD999999",
                Quantity = int.MaxValue,
                Price = decimal.MaxValue
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void OrderRequest_PropertiesSetCorrectly()
        {
            // Arrange
            var customerId = "CUST123";
            var productId = "PROD456";
            var quantity = 10;
            var price = 199.99m;

            // Act
            var orderRequest = new OrderRequest
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity,
                Price = price
            };

            // Assert
            orderRequest.CustomerId.Should().Be(customerId);
            orderRequest.ProductId.Should().Be(productId);
            orderRequest.Quantity.Should().Be(quantity);
            orderRequest.Price.Should().Be(price);
        }

        private static List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, validationResults, true);
            return validationResults;
        }
    }
}