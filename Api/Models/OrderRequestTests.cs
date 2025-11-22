using System;
using Xunit;
using FluentAssertions;

namespace Api.Models.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void Constructor_ValidData_ShouldCreateOrderRequest()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "Test Product",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(1);
            orderRequest.productname.Should().Be("Test Product");
            orderRequest.quantity.Should().Be(5);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Theory]
        [InlineData(0, "Product", 1, OrderStatus.IN_PROGRESS)]
        [InlineData(1, "", 2, OrderStatus.COMPLETED)]
        [InlineData(2, "Another Product", 0, OrderStatus.REJECTED)]
        public void OrderRequest_AllValidScenarios_ShouldSetProperties(int id, string productName, int quantity, OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = id,
                productname = productName,
                quantity = quantity,
                status = status
            };

            // Assert
            orderRequest.id.Should().Be(id);
            orderRequest.productname.Should().Be(productName);
            orderRequest.quantity.Should().Be(quantity);
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderStatus_AllEnumValues_ShouldBeValid()
        {
            // Arrange & Act
            var enumValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            enumValues.Should().Contain(OrderStatus.IN_PROGRESS);
            enumValues.Should().Contain(OrderStatus.COMPLETED);
            enumValues.Should().Contain(OrderStatus.REJECTED);
            enumValues.Length.Should().Be(3);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Theory]
        [InlineData(-1, "Product", 1, OrderStatus.IN_PROGRESS)]
        [InlineData(1, null, 2, OrderStatus.COMPLETED)]
        [InlineData(2, "Product", -5, OrderStatus.REJECTED)]
        public void OrderRequest_InvalidInputs_ShouldAllowCreation(int id, string productName, int quantity, OrderStatus status)
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = id,
                productname = productName,
                quantity = quantity,
                status = status
            };

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderStatus_EnumOrder_ShouldMatchExpectedSequence()
        {
            // Arrange
            var expectedOrder = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act
            var actualOrder = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            actualOrder.Should().Equal(expectedOrder);
        }

        [Fact]
        public void OrderRequest_PropertiesCanBeModified_ShouldUpdateSuccessfully()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = 100;
            orderRequest.productname = "Updated Product";
            orderRequest.quantity = 50;
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.id.Should().Be(100);
            orderRequest.productname.Should().Be("Updated Product");
            orderRequest.quantity.Should().Be(50);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var orderRequest1 = new OrderRequest { id = 1, productname = "Product1" };
            var orderRequest2 = new OrderRequest { id = 2, productname = "Product2" };

            // Assert
            orderRequest1.id.Should().NotBe(orderRequest2.id);
            orderRequest1.productname.Should().NotBe(orderRequest2.productname);
        }
    }
}