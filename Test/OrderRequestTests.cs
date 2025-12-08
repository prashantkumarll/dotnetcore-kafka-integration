using Xunit;
using Api.Models;
using FluentAssertions;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ShouldBeInstantiable()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.GetType().Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_ShouldHaveParameterlessConstructor()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_ShouldBeReference Type()
        {
            // Arrange
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Act & Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
            orderRequest1.Should().NotBeNull();
            orderRequest2.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_ShouldSupportSerialization()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(orderRequest);
            var deserializedOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderRequest>(json);

            // Assert
            deserializedOrder.Should().NotBeNull();
            deserializedOrder.Should().BeOfType<OrderRequest>();
        }
    }
}