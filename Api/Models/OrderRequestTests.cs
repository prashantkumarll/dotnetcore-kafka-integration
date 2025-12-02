using Xunit;
using FluentAssertions;
using Api.Models;
using System;
using System.Linq;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.id.Should().Be(0);
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_SetId_ShouldUpdateIdProperty()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 123;

            // Act
            orderRequest.id = expectedId;

            // Assert
            orderRequest.id.Should().Be(expectedId);
        }

        [Fact]
        public void OrderRequest_SetProductName_ShouldUpdateProductNameProperty()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedProductName = "TestProduct";

            // Act
            orderRequest.productname = expectedProductName;

            // Assert
            orderRequest.productname.Should().Be(expectedProductName);
        }

        [Fact]
        public void OrderRequest_SetQuantity_ShouldUpdateQuantityProperty()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedQuantity = 5;

            // Act
            orderRequest.quantity = expectedQuantity;

            // Assert
            orderRequest.quantity.Should().Be(expectedQuantity);
        }

        [Fact]
        public void OrderRequest_SetStatus_ShouldUpdateStatusProperty()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedStatus = OrderStatus.COMPLETED;

            // Act
            orderRequest.status = expectedStatus;

            // Assert
            orderRequest.status.Should().Be(expectedStatus);
        }

        [Theory]
        [InlineData(1, "Product1", 10, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 20, OrderStatus.COMPLETED)]
        [InlineData(3, "Product3", 30, OrderStatus.REJECTED)]
        public void OrderRequest_SetAllProperties_ShouldUpdateAllValues(int id, string productName, int quantity, OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = id;
            orderRequest.productname = productName;
            orderRequest.quantity = quantity;
            orderRequest.status = status;

            // Assert
            orderRequest.id.Should().Be(id);
            orderRequest.productname.Should().Be(productName);
            orderRequest.quantity.Should().Be(quantity);
            orderRequest.status.Should().Be(status);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void OrderRequest_SetIdBoundaryValues_ShouldAcceptAllIntegerValues(int id)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = id;

            // Assert
            orderRequest.id.Should().Be(id);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void OrderRequest_SetQuantityBoundaryValues_ShouldAcceptAllIntegerValues(int quantity)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = quantity;

            // Assert
            orderRequest.quantity.Should().Be(quantity);
        }

        [Fact]
        public void OrderRequest_SetProductNameToEmptyString_ShouldAcceptEmptyString()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var emptyProductName = string.Empty;

            // Act
            orderRequest.productname = emptyProductName;

            // Assert
            orderRequest.productname.Should().Be(emptyProductName);
        }

        [Fact]
        public void OrderRequest_SetProductNameToLongString_ShouldAcceptLongString()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var longProductName = new string('A', 1000);

            // Act
            orderRequest.productname = longProductName;

            // Assert
            orderRequest.productname.Should().Be(longProductName);
            orderRequest.productname.Length.Should().Be(1000);
        }

        [Fact]
        public void OrderRequest_SetProductNameWithSpecialCharacters_ShouldAcceptSpecialCharacters()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var specialProductName = "Product@#$%^&*()";

            // Act
            orderRequest.productname = specialProductName;

            // Assert
            orderRequest.productname.Should().Be(specialProductName);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SetStatusToAllValidValues_ShouldAcceptAllEnumValues(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = status;

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldContainAllExpectedValues()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act
            var actualValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            actualValues.Should().Contain(OrderStatus.IN_PROGRESS);
            actualValues.Should().Contain(OrderStatus.COMPLETED);
            actualValues.Should().Contain(OrderStatus.REJECTED);
            actualValues.Length.Should().Be(3);
        }

        [Fact]
        public void OrderRequest_MultiplePropertyUpdates_ShouldMaintainIndependentValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = 100;
            orderRequest.productname = "InitialProduct";
            orderRequest.quantity = 50;
            orderRequest.status = OrderStatus.IN_PROGRESS;

            orderRequest.productname = "UpdatedProduct";
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.id.Should().Be(100);
            orderRequest.productname.Should().Be("UpdatedProduct");
            orderRequest.quantity.Should().Be(50);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void OrderRequest_CreateMultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var order1 = new OrderRequest { id = 1, productname = "Product1", quantity = 10, status = OrderStatus.IN_PROGRESS };
            var order2 = new OrderRequest { id = 2, productname = "Product2", quantity = 20, status = OrderStatus.COMPLETED };

            // Assert
            order1.id.Should().Be(1);
            order1.productname.Should().Be("Product1");
            order1.quantity.Should().Be(10);
            order1.status.Should().Be(OrderStatus.IN_PROGRESS);

            order2.id.Should().Be(2);
            order2.productname.Should().Be("Product2");
            order2.quantity.Should().Be(20);
            order2.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}