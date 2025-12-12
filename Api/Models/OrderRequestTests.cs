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
        public void Productname_SetAndGet_ShouldReturnCorrectValue()
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
        public void Id_SetVariousValues_ShouldReturnCorrectValue(int testId)
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
        public void Quantity_SetVariousValues_ShouldReturnCorrectValue(int testQuantity)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = testQuantity;

            // Assert
            orderRequest.quantity.Should().Be(testQuantity);
        }

        [Fact]
        public void Status_SetToInProgress_ShouldReturnInProgress()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = OrderStatus.IN_PROGRESS;

            // Assert
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void Status_SetToCompleted_ShouldReturnCompleted()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void Status_SetToRejected_ShouldReturnRejected()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = OrderStatus.REJECTED;

            // Assert
            orderRequest.status.Should().Be(OrderStatus.REJECTED);
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
            var expectedId = 42;
            var expectedProductName = "Widget";
            var expectedQuantity = 10;
            var expectedStatus = OrderStatus.COMPLETED;

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
    }

    public class OrderStatusTests
    {
        [Fact]
        public void OrderStatus_ShouldHaveInProgressValue()
        {
            // Arrange & Act
            var status = OrderStatus.IN_PROGRESS;

            // Assert
            status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderStatus_ShouldHaveCompletedValue()
        {
            // Arrange & Act
            var status = OrderStatus.COMPLETED;

            // Assert
            status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void OrderStatus_ShouldHaveRejectedValue()
        {
            // Arrange & Act
            var status = OrderStatus.REJECTED;

            // Assert
            status.Should().Be(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderStatus_AllValues_ShouldBeAccessible()
        {
            // Arrange
            var allStatuses = System.Enum.GetValues(typeof(OrderStatus));
            var statusArray = new OrderStatus[allStatuses.Length];
            
            // Act
            for (int i = 0; i < allStatuses.Length; i++)
            {
                statusArray[i] = (OrderStatus)allStatuses.GetValue(i);
            }

            // Assert
            statusArray.Should().HaveCount(3);
            statusArray.Should().Contain(OrderStatus.IN_PROGRESS);
            statusArray.Should().Contain(OrderStatus.COMPLETED);
            statusArray.Should().Contain(OrderStatus.REJECTED);
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