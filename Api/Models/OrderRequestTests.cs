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
        public void OrderRequest_Constructor_Should_CreateInstance()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void Id_Property_Should_SetAndGetValue()
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
        public void ProductName_Property_Should_SetAndGetValue()
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
        public void Quantity_Property_Should_SetAndGetValue()
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
        public void Status_Property_Should_SetAndGetValue()
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
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Id_Property_Should_AcceptNegativeAndZeroValues(int testId)
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
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Quantity_Property_Should_AcceptVariousIntegerValues(int testQuantity)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.quantity = testQuantity;

            // Assert
            orderRequest.quantity.Should().Be(testQuantity);
        }

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void Status_Property_Should_AcceptAllValidEnumValues(OrderStatus testStatus)
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            orderRequest.status = testStatus;

            // Assert
            orderRequest.status.Should().Be(testStatus);
        }

        [Fact]
        public void OrderRequest_Should_InitializeWithDefaultValues()
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
        public void OrderRequest_Should_AllowSettingAllPropertiesAtOnce()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedId = 456;
            var expectedProductName = "Sample Product";
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
        public void ProductName_Property_Should_AcceptEmptyString()
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
        public void ProductName_Property_Should_AcceptLongString()
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
        public void OrderStatus_Enum_Should_ContainAllExpectedValues()
        {
            // Arrange
            var expectedValues = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };

            // Act
            var actualValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            actualValues.Should().BeEquivalentTo(expectedValues);
        }

        [Fact]
        public void OrderStatus_Enum_Should_HaveCorrectUnderlyingValues()
        {
            // Arrange & Act & Assert
            ((int)OrderStatus.IN_PROGRESS).Should().Be(0);
            ((int)OrderStatus.COMPLETED).Should().Be(1);
            ((int)OrderStatus.REJECTED).Should().Be(2);
        }

        [Fact]
        public void OrderRequest_Properties_Should_BeIndependent()
        {
            // Arrange
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Act
            orderRequest1.id = 100;
            orderRequest1.productname = "Product1";
            orderRequest1.quantity = 5;
            orderRequest1.status = OrderStatus.COMPLETED;

            orderRequest2.id = 200;
            orderRequest2.productname = "Product2";
            orderRequest2.quantity = 10;
            orderRequest2.status = OrderStatus.REJECTED;

            // Assert
            orderRequest1.id.Should().Be(100);
            orderRequest1.productname.Should().Be("Product1");
            orderRequest1.quantity.Should().Be(5);
            orderRequest1.status.Should().Be(OrderStatus.COMPLETED);

            orderRequest2.id.Should().Be(200);
            orderRequest2.productname.Should().Be("Product2");
            orderRequest2.quantity.Should().Be(10);
            orderRequest2.status.Should().Be(OrderStatus.REJECTED);
        }
    }
}