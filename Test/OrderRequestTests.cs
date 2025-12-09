using Xunit;
using FluentAssertions;
using Api.Models;
using System;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_Should_BeInstantiable()
        {
            // Arrange & Act
            var orderRequest = new OrderRequest();

            // Assert
            orderRequest.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_Should_BeReferenceType()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_Should_BeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_Should_HaveParameterlessConstructor()
        {
            // Arrange & Act
            var constructor = typeof(OrderRequest).GetConstructor(Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_Multiple_Instances_Should_Be_Different()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
        }

        [Fact]
        public void OrderRequest_Should_Be_Serializable()
        {
            // Arrange
            var orderRequest = new OrderRequest();

            // Act & Assert
            FluentActions.Invoking(() => Newtonsoft.Json.JsonConvert.SerializeObject(orderRequest))
                .Should().NotThrow();
        }
    }
}