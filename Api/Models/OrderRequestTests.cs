using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Constructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(1);
            orderRequest.productname.Should().Be("TestProduct");
            orderRequest.quantity.Should().Be(10);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_StatusEnum_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SetStatus_ShouldUpdateCorrectly(OrderStatus status)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = status;

            // Assert
            orderRequest.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldInitializeEmptyObject()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(default(OrderStatus));
        }

        [Theory]
        [InlineData(1, "Product1", 5, OrderStatus.IN_PROGRESS)]
        [InlineData(2, "Product2", 10, OrderStatus.COMPLETED)]
        [InlineData(3, "Product3", 15, OrderStatus.REJECTED)]
        public void OrderRequest_MultipleInstances_ShouldHaveDifferentValues(int id, string productName, int quantity, OrderStatus status)
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
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = null,
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.productname.Should().BeNull();
        }

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "TestProduct",
                quantity = -5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.quantity.Should().Be(-5);
        }

        [Fact]
        public void OrderRequest_ZeroId_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 0,
                productname = "TestProduct",
                quantity = 10,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.id.Should().Be(0);
        }

        [Fact]
        public void OrderRequest_EmptyProductName_ShouldBeAllowed()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.productname.Should().Be("");
        }

        [Fact]
        public void OrderRequest_MaxIntValues_ShouldBeHandled()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = int.MaxValue,
                productname = "MaxProduct",
                quantity = int.MaxValue,
                status = OrderStatus.COMPLETED
            };

            // Assert
            orderRequest.id.Should().Be(int.MaxValue);
            orderRequest.quantity.Should().Be(int.MaxValue);
        }

        [Fact]
        public void OrderRequest_MinIntValues_ShouldBeHandled()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = int.MinValue,
                productname = "MinProduct",
                quantity = int.MinValue,
                status = OrderStatus.REJECTED
            };

            // Assert
            orderRequest.id.Should().Be(int.MinValue);
            orderRequest.quantity.Should().Be(int.MinValue);
        }

        [Fact]
        public void OrderStatus_EnumCount_ShouldHaveThreeValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statusValues.Should().HaveCount(3);
        }

        [Fact]
        public void OrderStatus_EnumNames_ShouldMatchExpectedNames()
        {
            // Arrange & Act
            var statusNames = Enum.GetNames(typeof(OrderStatus));

            // Assert
            statusNames.Should().Contain("IN_PROGRESS");
            statusNames.Should().Contain("COMPLETED");
            statusNames.Should().Contain("REJECTED");
        }

        [Theory]
        [InlineData(0, OrderStatus.IN_PROGRESS)]
        [InlineData(1, OrderStatus.COMPLETED)]
        [InlineData(2, OrderStatus.REJECTED)]
        public void OrderStatus_EnumValues_ShouldHaveCorrectIntegerValues(int expectedValue, OrderStatus status)
        {
            // Arrange & Act
            var actualValue = (int)status;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        [Fact]
        public void OrderRequest_PropertyModification_ShouldUpdateIndependently()
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
            orderRequest.id = 999;
            orderRequest.productname = "UpdatedProduct";
            orderRequest.quantity = 100;
            orderRequest.status = OrderStatus.COMPLETED;

            // Assert
            orderRequest.id.Should().Be(999);
            orderRequest.productname.Should().Be("UpdatedProduct");
            orderRequest.quantity.Should().Be(100);
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}