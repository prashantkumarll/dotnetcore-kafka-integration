using Xunit;
using FluentAssertions;
using Api.Models;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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
            var expectedProductName = "Test Product";

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

        [Theory]
        [InlineData("Product A")]
        [InlineData("Product B")]
        [InlineData("")]
        public void ProductName_SetVariousStrings_ShouldAcceptAllValues(string testProductName)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.productname = testProductName;

            // Assert
            orderRequest.productname.Should().Be(testProductName);
        }

        [Fact]
        public void ProductName_SetNull_ShouldAcceptNullValue()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.productname = default!;

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void Status_SetAllEnumValues_ShouldAcceptValidStatuses(OrderStatus testStatus)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = testStatus;

            // Assert
            orderRequest.status.Should().Be(testStatus);
        }

        [Fact]
        public void OrderRequest_SetAllProperties_ShouldRetainAllValues()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 42;
            var expectedProductName = "Complete Product";
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
        public void OrderRequest_DefaultValues_ShouldHaveExpectedDefaults()
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
        public void OrderStatus_EnumValues_ShouldContainAllExpectedValues()
        {
            // Arrange
            var enumValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Act & Assert
            enumValues.Should().HaveCount(3);
            enumValues.Should().Contain(OrderStatus.IN_PROGRESS);
            enumValues.Should().Contain(OrderStatus.COMPLETED);
            enumValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderStatus_ToString_ShouldReturnCorrectStringRepresentation()
        {
            // Arrange & Act & Assert
            OrderStatus.IN_PROGRESS.ToString().Should().Be("IN_PROGRESS");
            OrderStatus.COMPLETED.ToString().Should().Be("COMPLETED");
            OrderStatus.REJECTED.ToString().Should().Be("REJECTED");
        }

        [Fact]
        public void OrderRequest_ObjectEquality_ShouldWorkCorrectly()
        {
            // Arrange
            var order1 = new OrderRequest
            {
                id = 1,
                productname = "Test Product",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            var order2 = new OrderRequest
            {
                id = 1,
                productname = "Test Product",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Act & Assert
            order1.Should().BeEquivalentTo(order2);
        }

        [Fact]
        public void OrderRequest_PropertyInitialization_ShouldAllowObjectInitializer()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 999,
                productname = "Initialized Product",
                quantity = 25,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.id.Should().Be(999);
            orderRequest.productname.Should().Be("Initialized Product");
            orderRequest.quantity.Should().Be(25);
            orderRequest.status.Should().Be(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderStatus_EnumUnderlyingValues_ShouldBeSequential()
        {
            // Arrange & Act & Assert
            ((int)OrderStatus.IN_PROGRESS).Should().Be(0);
            ((int)OrderStatus.COMPLETED).Should().Be(1);
            ((int)OrderStatus.REJECTED).Should().Be(2);
        }

        [Fact]
        public void OrderRequest_PropertyTypes_ShouldBeCorrect()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act & Assert
            orderRequest.id.Should().BeOfType<int>();
            orderRequest.quantity.Should().BeOfType<int>();
            orderRequest.status.Should().BeOfType<OrderStatus>();
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Id_SetBoundaryValues_ShouldAcceptExtremeValues(int boundaryValue)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.id = boundaryValue;

            // Assert
            orderRequest.id.Should().Be(boundaryValue);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Quantity_SetBoundaryValues_ShouldAcceptExtremeValues(int boundaryValue)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = boundaryValue;

            // Assert
            orderRequest.quantity.Should().Be(boundaryValue);
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var order1 = new OrderRequest { id = 1, productname = "Product1", quantity = 10, status = OrderStatus.IN_PROGRESS };
            var order2 = new OrderRequest { id = 2, productname = "Product2", quantity = 20, status = OrderStatus.COMPLETED };

            // Act
            order1.id = 100;
            order1.productname = "Modified Product";

            // Assert
            order1.id.Should().Be(100);
            order1.productname.Should().Be("Modified Product");
            order2.id.Should().Be(2);
            order2.productname.Should().Be("Product2");
        }

        [Fact]
        public void OrderStatus_ParseFromString_ShouldWorkCorrectly()
        {
            // Arrange & Act & Assert
            Enum.Parse<OrderStatus>("IN_PROGRESS").Should().Be(OrderStatus.IN_PROGRESS);
            Enum.Parse<OrderStatus>("COMPLETED").Should().Be(OrderStatus.COMPLETED);
            Enum.Parse<OrderStatus>("REJECTED").Should().Be(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderStatus_IsDefined_ShouldReturnTrueForValidValues()
        {
            // Arrange & Act & Assert
            Enum.IsDefined(typeof(OrderStatus), OrderStatus.IN_PROGRESS).Should().BeTrue();
            Enum.IsDefined(typeof(OrderStatus), OrderStatus.COMPLETED).Should().BeTrue();
            Enum.IsDefined(typeof(OrderStatus), OrderStatus.REJECTED).Should().BeTrue();
        }
    }
}