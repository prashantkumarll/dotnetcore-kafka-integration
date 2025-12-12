using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
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

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(-1)]
        [InlineData(0)]
        public void OrderRequest_SetId_ShouldAcceptVariousIntegerValues(int testId)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = testId;

            // Assert
            orderRequest.id.Should().Be(testId);
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

        [Theory]
        [InlineData("Laptop")]
        [InlineData("Mouse")]
        [InlineData("Keyboard")]
        [InlineData("")]
        public void OrderRequest_SetProductName_ShouldAcceptVariousStringValues(string productName)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.productname = productName;

            // Assert
            orderRequest.productname.Should().Be(productName);
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

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(0)]
        [InlineData(-1)]
        public void OrderRequest_SetQuantity_ShouldAcceptVariousIntegerValues(int testQuantity)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = testQuantity;

            // Assert
            orderRequest.quantity.Should().Be(testQuantity);
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
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SetStatus_ShouldAcceptAllValidOrderStatusValues(OrderStatus testStatus)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = testStatus;

            // Assert
            orderRequest.status.Should().Be(testStatus);
        }

        [Fact]
        public void OrderRequest_SetAllProperties_ShouldUpdateAllPropertiesCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 456;
            var expectedProductName = "TestProduct";
            var expectedQuantity = 10;
            var expectedStatus = OrderStatus.REJECTED;

            // Act
            orderRequest.id = expectedId;
            orderRequest.productname = expectedProductName;
            orderRequest.quantity = expectedQuantity;
            orderRequest.status = expectedStatus;

            // Assert
            orderRequest.id.Should().Be(expectedId);
            orderRequest.productname.Should().Be(expectedProductName);
            orderRequest.quantity.Should().Be(expectedQuantity);
            orderRequest.status.Should().Be(expectedStatus);
        }

        [Fact]
        public void OrderStatus_Enum_ShouldContainAllExpectedValues()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act
            var actualValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            actualValues.Should().BeEquivalentTo(expectedValues);
        }

        [Fact]
        public void OrderStatus_IN_PROGRESS_ShouldHaveCorrectValue()
        {
            // Arrange & Act
            var status = OrderStatus.IN_PROGRESS;

            // Assert
            status.Should().Be(OrderStatus.IN_PROGRESS);
            ((int)status).Should().Be(0);
        }

        [Fact]
        public void OrderStatus_COMPLETED_ShouldHaveCorrectValue()
        {
            // Arrange & Act
            var status = OrderStatus.COMPLETED;

            // Assert
            status.Should().Be(OrderStatus.COMPLETED);
            ((int)status).Should().Be(1);
        }

        [Fact]
        public void OrderStatus_REJECTED_ShouldHaveCorrectValue()
        {
            // Arrange & Act
            var status = OrderStatus.REJECTED;

            // Assert
            status.Should().Be(OrderStatus.REJECTED);
            ((int)status).Should().Be(2);
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var order1 = new OrderRequest();
            var order2 = new OrderRequest();

            // Act
            order1.id = 1;
            order1.productname = "Product1";
            order1.quantity = 5;
            order1.status = OrderStatus.COMPLETED;

            order2.id = 2;
            order2.productname = "Product2";
            order2.quantity = 10;
            order2.status = OrderStatus.REJECTED;

            // Assert
            order1.id.Should().Be(1);
            order1.productname.Should().Be("Product1");
            order1.quantity.Should().Be(5);
            order1.status.Should().Be(OrderStatus.COMPLETED);

            order2.id.Should().Be(2);
            order2.productname.Should().Be("Product2");
            order2.quantity.Should().Be(10);
            order2.status.Should().Be(OrderStatus.REJECTED);
        }
    }
}