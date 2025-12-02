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
        public void OrderRequest_SetAllProperties_ShouldReturnAllCorrectValues()
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
        public void OrderStatus_AllEnumValues_ShouldBeAccessible()
        {
            // Arrange
            var enumValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Act & Assert
            enumValues.Should().Contain(OrderStatus.IN_PROGRESS);
            enumValues.Should().Contain(OrderStatus.COMPLETED);
            enumValues.Should().Contain(OrderStatus.REJECTED);
            enumValues.Length.Should().Be(3);
        }

        [Fact]
        public void OrderStatus_EnumValues_ShouldHaveCorrectIntegerValues()
        {
            // Arrange & Act & Assert
            ((int)OrderStatus.IN_PROGRESS).Should().Be(0);
            ((int)OrderStatus.COMPLETED).Should().Be(1);
            ((int)OrderStatus.REJECTED).Should().Be(2);
        }
    }
}