using Api.Models;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_CanBeInstantiated()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_IsReferenceType()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
        }

        [Fact]
        public void OrderRequest_CanBeAssignedToVariable()
        {
            // Arrange & Act
            OrderRequest orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_CreatesValidInstance()
        {
            // Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.GetType().Should().Be<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_MultipleInstances_AreDistinct()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();
            var orderRequest3 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
            orderRequest2.Should().NotBeSameAs(orderRequest3);
            orderRequest1.Should().NotBeSameAs(orderRequest3);
        }
    }
}