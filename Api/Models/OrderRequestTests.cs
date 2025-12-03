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
        public void Id_SetDifferentValues_ShouldReturnCorrectValue(int testId)
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
        public void Quantity_SetDifferentValues_ShouldReturnCorrectValue(int testQuantity)
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

        [Fact]
        public void OrderStatus_EnumValues_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var inProgressValue = OrderStatus.IN_PROGRESS;
            var completedValue = OrderStatus.COMPLETED;
            var rejectedValue = OrderStatus.REJECTED;

            // Assert
            inProgressValue.Should().Be(OrderStatus.IN_PROGRESS);
            completedValue.Should().Be(OrderStatus.COMPLETED);
            rejectedValue.Should().Be(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderStatus_EnumCasting_ShouldWorkCorrectly()
        {
            // Arrange & Act
            var inProgressInt = (int)OrderStatus.IN_PROGRESS;
            var completedInt = (int)OrderStatus.COMPLETED;
            var rejectedInt = (int)OrderStatus.REJECTED;

            // Assert
            inProgressInt.Should().Be(0);
            completedInt.Should().Be(1);
            rejectedInt.Should().Be(2);
        }

        [Fact]
        public void OrderRequest_PropertyModification_ShouldNotAffectOtherProperties()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "InitialProduct",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Act
            orderRequest.productname = "ModifiedProduct";

            // Assert
            orderRequest.id.Should().Be(1);
            orderRequest.productname.Should().Be("ModifiedProduct");
            orderRequest.quantity.Should().Be(5);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }
    }
}