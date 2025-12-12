using Xunit;
using FluentAssertions;
using Api.Models;
using System;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void Id_SetAndGet_ShouldReturnCorrectValue()
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
        public void ProductName_SetAndGet_ShouldReturnCorrectValue()
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
        public void Quantity_SetAndGet_ShouldReturnCorrectValue()
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
        public void Status_SetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedStatus = OrderStatus.IN_PROGRESS;

            // Act
            orderRequest.status = expectedStatus;

            // Assert
            orderRequest.status.Should().Be(expectedStatus);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(-1)]
        public void Id_SetVariousValues_ShouldAcceptAllIntegers(int testId)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = testId;

            // Assert
            orderRequest.id.Should().Be(testId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(-5)]
        public void Quantity_SetVariousValues_ShouldAcceptAllIntegers(int testQuantity)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = testQuantity;

            // Assert
            orderRequest.quantity.Should().Be(testQuantity);
        }

        [Fact]
        public void ProductName_SetEmptyString_ShouldAcceptEmptyString()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var emptyString = "";

            // Act
            orderRequest.productname = emptyString;

            // Assert
            orderRequest.productname.Should().Be(emptyString);
        }

        [Fact]
        public void OrderRequest_DefaultValues_ShouldHaveExpectedDefaults()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_SetAllProperties_ShouldRetainAllValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var testId = 42;
            var testProductName = "Widget";
            var testQuantity = 10;
            var testStatus = OrderStatus.COMPLETED;

            // Act
            orderRequest.id = testId;
            orderRequest.productname = testProductName;
            orderRequest.quantity = testQuantity;
            orderRequest.status = testStatus;

            // Assert
            orderRequest.id.Should().Be(testId);
            orderRequest.productname.Should().Be(testProductName);
            orderRequest.quantity.Should().Be(testQuantity);
            orderRequest.status.Should().Be(testStatus);
        }
    }

    public class OrderStatusTests
    {
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
        public void OrderStatus_AllValues_ShouldContainExpectedValues()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act
            var allValues = System.Enum.GetValues(typeof(OrderStatus));
            var typedValues = new OrderStatus[allValues.Length];
            for (int i = 0; i < allValues.Length; i++)
            {
                typedValues[i] = (OrderStatus)allValues.GetValue(i);
            }

            // Assert
            typedValues.Should().HaveCount(3);
            typedValues.Should().Contain(OrderStatus.IN_PROGRESS);
            typedValues.Should().Contain(OrderStatus.COMPLETED);
            typedValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderStatus_ValidValues_ShouldBeAssignable(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = status;

            // Assert
            orderRequest.status.Should().Be(status);
        }
    }
}