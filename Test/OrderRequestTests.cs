using Xunit;
using FluentAssertions;
using Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                CustomerId = "CUST-001",
                Amount = 100.50m,
                ProductName = "Valid Product"
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
        public void OrderRequest_WithInvalidOrderId_ShouldFailValidation(string orderId)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = orderId,
                CustomerId = "CUST-001",
                Amount = 100.50m,
                ProductName = "Product"
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
        public void OrderRequest_WithInvalidCustomerId_ShouldFailValidation(string customerId)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                CustomerId = customerId,
                Amount = 100.50m,
                ProductName = "Product"
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("CustomerId"));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void OrderRequest_WithInvalidAmount_ShouldFailValidation(decimal amount)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                CustomerId = "CUST-001",
                Amount = amount,
                ProductName = "Product"
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Amount"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void OrderRequest_WithInvalidProductName_ShouldFailValidation(string productName)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = "ORD-001",
                CustomerId = "CUST-001",
                Amount = 100.50m,
                ProductName = productName
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("ProductName"));
        }

        [Fact]
        public void OrderRequest_WithMaxValues_ShouldPassValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                OrderId = new string('A', 100),
                CustomerId = new string('B', 100),
                Amount = decimal.MaxValue,
                ProductName = new string('C', 200)
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
            var expectedOrderId = "ORD-123";
            var expectedCustomerId = "CUST-456";
            var expectedAmount = 299.99m;
            var expectedProductName = "Test Product Name";

            // Act
            var orderRequest = new OrderRequest
            {
                OrderId = expectedOrderId,
                CustomerId = expectedCustomerId,
                Amount = expectedAmount,
                ProductName = expectedProductName
            };

            // Assert
            orderRequest.OrderId.Should().Be(expectedOrderId);
            orderRequest.CustomerId.Should().Be(expectedCustomerId);
            orderRequest.Amount.Should().Be(expectedAmount);
            orderRequest.ProductName.Should().Be(expectedProductName);
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}