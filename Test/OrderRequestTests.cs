using Xunit;
using FluentAssertions;
using Api.Models;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldCreateInstance()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_Properties_ShouldBeSettableAndGettable()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedOrderId = "ORD-12345";
            var expectedCustomerName = "John Doe";
            var expectedAmount = 99.99m;

            // Act
            orderRequest.OrderId = expectedOrderId;
            orderRequest.CustomerName = expectedCustomerName;
            orderRequest.Amount = expectedAmount;

            // Assert
            orderRequest.OrderId.Should().Be(expectedOrderId);
            orderRequest.CustomerName.Should().Be(expectedCustomerName);
            orderRequest.Amount.Should().Be(expectedAmount);
        }

        [Fact]
        public void OrderRequest_WithNullValues_ShouldAcceptNullProperties()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                OrderId = null,
                CustomerName = null,
                Amount = 0
            };

            // Assert
            orderRequest.OrderId.Should().BeNull();
            orderRequest.CustomerName.Should().BeNull();
            orderRequest.Amount.Should().Be(0);
        }

        [Fact]
        public void OrderRequest_WithEmptyStrings_ShouldAcceptEmptyProperties()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                OrderId = string.Empty,
                CustomerName = string.Empty
            };

            // Assert
            orderRequest.OrderId.Should().BeEmpty();
            orderRequest.CustomerName.Should().BeEmpty();
        }

        [Fact]
        public void OrderRequest_TypeCheck_ShouldBeOfCorrectType()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().BeOfType<OrderRequest>();
            orderRequest.GetType().Should().Be(typeof(OrderRequest));
        }
    }
}