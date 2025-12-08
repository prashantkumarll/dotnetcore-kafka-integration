using Xunit;
using FluentAssertions;
using Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class OrderRequestModelTests
    {
        [Fact]
        public void OrderRequest_ValidData_PassesValidation()
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 5,
                Price = 99.99m
            };

            // Act
            var validationResults = ValidateModel(order);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void OrderRequest_InvalidCustomerId_FailsValidation(string customerId)
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = customerId,
                ProductId = "PROD001",
                Quantity = 1,
                Price = 10.00m
            };

            // Act
            var validationResults = ValidateModel(order);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("CustomerId"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void OrderRequest_InvalidProductId_FailsValidation(string productId)
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = productId,
                Quantity = 1,
                Price = 10.00m
            };

            // Act
            var validationResults = ValidateModel(order);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("ProductId"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void OrderRequest_InvalidQuantity_FailsValidation(int quantity)
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = quantity,
                Price = 10.00m
            };

            // Act
            var validationResults = ValidateModel(order);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Quantity"));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-0.01)]
        [InlineData(-100.50)]
        public void OrderRequest_InvalidPrice_FailsValidation(decimal price)
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "CUST001",
                ProductId = "PROD001",
                Quantity = 1,
                Price = price
            };

            // Act
            var validationResults = ValidateModel(order);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Price"));
        }

        [Fact]
        public void OrderRequest_ValidBoundaryValues_PassesValidation()
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "C",
                ProductId = "P",
                Quantity = 1,
                Price = 0.01m
            };

            // Act
            var validationResults = ValidateModel(order);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void OrderRequest_LargeValidValues_PassesValidation()
        {
            // Arrange
            var order = new OrderRequest
            {
                CustomerId = "CUSTOMER_WITH_VERY_LONG_ID_12345",
                ProductId = "PRODUCT_WITH_VERY_LONG_ID_67890",
                Quantity = int.MaxValue,
                Price = decimal.MaxValue
            };

            // Act
            var validationResults = ValidateModel(order);

            // Assert
            validationResults.Should().BeEmpty();
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}