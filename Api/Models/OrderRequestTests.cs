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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Id_SetVariousValues_ShouldReturnCorrectValue(int testId)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = testId;

            // Assert
            orderRequest.id.Should().Be(testId);
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

        [Theory]
        [InlineData("Product1")]
        [InlineData("Product with spaces")]
        [InlineData("Product123")]
        [InlineData("")]
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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
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

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void Status_SetAllValidValues_ShouldReturnCorrectValue(OrderStatus testStatus)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = testStatus;

            // Assert
            orderRequest.status.Should().Be(testStatus);
        }

        [Fact]
        public void OrderRequest_SetAllProperties_ShouldReturnAllCorrectValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 42;
            var expectedProductName = "TestProduct";
            var expectedQuantity = 10;
            var expectedStatus = OrderStatus.IN_PROGRESS;

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
        public void OrderRequest_ObjectEquality_ShouldWorkCorrectly()
        {
            // Arrange
            var orderRequest1 = new OrderRequest
            {
                id = 1,
                productname = "Product1",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            var orderRequest2 = new OrderRequest
            {
                id = 1,
                productname = "Product1",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Act & Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
            orderRequest1.id.Should().Be(orderRequest2.id);
            orderRequest1.productname.Should().Be(orderRequest2.productname);
            orderRequest1.quantity.Should().Be(orderRequest2.quantity);
            orderRequest1.status.Should().Be(orderRequest2.status);
        }
    }
}