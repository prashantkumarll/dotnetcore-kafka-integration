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
            var orderRequest = new OrderRequest
            {
                Id = "ORD-123",
                CustomerName = "John Doe",
                Product = "Laptop",
                Quantity = 1,
                Price = 999.99m
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
        public void OrderRequest_InvalidId_FailsValidation(string id)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = id,
                CustomerName = "John Doe",
                Product = "Laptop",
                Quantity = 1,
                Price = 999.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Id"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void OrderRequest_InvalidCustomerName_FailsValidation(string customerName)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = "ORD-123",
                CustomerName = customerName,
                Product = "Laptop",
                Quantity = 1,
                Price = 999.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("CustomerName"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void OrderRequest_InvalidProduct_FailsValidation(string product)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = "ORD-123",
                CustomerName = "John Doe",
                Product = product,
                Quantity = 1,
                Price = 999.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Product"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void OrderRequest_InvalidQuantity_FailsValidation(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = "ORD-123",
                CustomerName = "John Doe",
                Product = "Laptop",
                Quantity = quantity,
                Price = 999.99m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Quantity"));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-0.01)]
        public void OrderRequest_NegativePrice_FailsValidation(double price)
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = "ORD-123",
                CustomerName = "John Doe",
                Product = "Laptop",
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
        public void OrderRequest_ZeroPrice_PassesValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = "ORD-123",
                CustomerName = "John Doe",
                Product = "Free Sample",
                Quantity = 1,
                Price = 0m
            };

            // Act
            var validationResults = ValidateModel(orderRequest);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void OrderRequest_MaxValues_PassesValidation()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                Id = new string('A', 100),
                CustomerName = new string('B', 200),
                Product = new string('C', 300),
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
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                Id = "ORD-456",
                CustomerName = "Jane Smith",
                Product = "Phone",
                Quantity = 2,
                Price = 599.99m
            };

            // Assert
            orderRequest.Id.Should().Be("ORD-456");
            orderRequest.CustomerName.Should().Be("Jane Smith");
            orderRequest.Product.Should().Be("Phone");
            orderRequest.Quantity.Should().Be(2);
            orderRequest.Price.Should().Be(599.99m);
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