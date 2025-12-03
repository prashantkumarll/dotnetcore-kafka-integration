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

        [Theory]
        [InlineData("Product1")]
        [InlineData("Product2")]
        [InlineData("TestProduct")]
        public void ProductName_SetVariousValues_ShouldReturnCorrectValue(string testProductName)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.productname = testProductName;

            // Assert
            orderRequest.productname.Should().Be(testProductName);
        }

        [Fact]
        public void OrderRequest_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_SetAllProperties_ShouldReturnCorrectValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 456;
            var expectedProductName = "CompleteProduct";
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
        public void OrderStatus_AllValues_ShouldBeAccessible()
        {
            // Arrange
            var allStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Act & Assert
            allStatuses.Should().Contain(OrderStatus.IN_PROGRESS);
            allStatuses.Should().Contain(OrderStatus.COMPLETED);
            allStatuses.Should().Contain(OrderStatus.REJECTED);
            allStatuses.Length.Should().Be(3);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void Status_SetAllEnumValues_ShouldReturnCorrectValue(OrderStatus testStatus)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = testStatus;

            // Assert
            orderRequest.status.Should().Be(testStatus);
        }
    }
}