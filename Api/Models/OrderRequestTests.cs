using Xunit;
using FluentAssertions;
using Api.Models;

namespace Api.Tests.Models
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldCreateValidInstance()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = "Test Product",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(1);
            orderRequest.productname.Should().Be("Test Product");
            orderRequest.quantity.Should().Be(5);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderStatus_ShouldContainAllExpectedStatuses()
        {
            // Arrange & Act
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statuses.Should().Contain(OrderStatus.IN_PROGRESS);
            statuses.Should().Contain(OrderStatus.COMPLETED);
            statuses.Should().Contain(OrderStatus.REJECTED);
            statuses.Length.Should().Be(3);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldInitializePropertiesToDefault()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.id.Should().Be(0);
            orderRequest.productname.Should().BeNull();
            orderRequest.quantity.Should().Be(0);
            orderRequest.status.Should().Be(OrderStatus.IN_PROGRESS);
        }
    }
}