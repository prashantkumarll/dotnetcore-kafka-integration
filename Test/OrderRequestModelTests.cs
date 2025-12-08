using Xunit;
using FluentAssertions;
using Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Test.Models
{
    public class OrderRequestModelTests
    {
        [Fact]
        public void OrderRequest_WithValidData_PassesValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-12345",
                CustomerId = "CUST-67890",
                ProductId = "PROD-ABCDE",
                Quantity = 3,
                Price = 99.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void OrderRequest_WithInvalidOrderId_FailsValidation(string? orderId)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = orderId!,
                CustomerId = "CUST-67890",
                ProductId = "PROD-ABCDE",
                Quantity = 3,
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
        [InlineData("   ")]
        public void OrderRequest_WithInvalidCustomerId_FailsValidation(string? customerId)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-12345",
                CustomerId = customerId!,
                ProductId = "PROD-ABCDE",
                Quantity = 3,
                Price = 99.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("CustomerId"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void OrderRequest_WithInvalidProductId_FailsValidation(string? productId)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-12345",
                CustomerId = "CUST-67890",
                ProductId = productId!,
                Quantity = 3,
                Price = 99.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("ProductId"));
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
                OrderId = "ORD-12345",
                CustomerId = "CUST-67890",
                ProductId = "PROD-ABCDE",
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
        [InlineData(0)]
        [InlineData(-1.50)]
        [InlineData(-100.99)]
        public void OrderRequest_WithInvalidPrice_FailsValidation(decimal price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-12345",
                CustomerId = "CUST-67890",
                ProductId = "PROD-ABCDE",
                Quantity = 1,
                Price = price
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Price"));
        }

        [Fact]
        public void OrderRequest_PropertyGettersAndSetters_WorkCorrectly()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                OrderId = "TEST-ORDER-123",
                CustomerId = "TEST-CUSTOMER-456",
                ProductId = "TEST-PRODUCT-789",
                Quantity = 5,
                Price = 299.99m
            };

            // Assert
            orderRequest.OrderId.Should().Be("TEST-ORDER-123");
            orderRequest.CustomerId.Should().Be("TEST-CUSTOMER-456");
            orderRequest.ProductId.Should().Be("TEST-PRODUCT-789");
            orderRequest.Quantity.Should().Be(5);
            orderRequest.Price.Should().Be(299.99m);
        }

        [Theory]
        [InlineData(1, 0.01)]
        [InlineData(999, 9999.99)]
        [InlineData(100, 50.50)]
        public void OrderRequest_WithValidQuantityAndPrice_PassesValidation(int quantity, decimal price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-12345",
                CustomerId = "CUST-67890",
                ProductId = "PROD-ABCDE",
                Quantity = quantity,
                Price = price
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().BeEmpty();
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}