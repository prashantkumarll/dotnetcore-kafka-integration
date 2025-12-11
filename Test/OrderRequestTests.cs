using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Api.Models;

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
            orderRequest.Should().BeOfType<OrderRequest>();
        }

        [Fact]
        public void OrderRequest_Type_ShouldHaveExpectedNamespace()
        {
            // Arrange & Act
            var type = typeof(OrderRequest);

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("OrderRequest");
            type.Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Act & Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
        }

        [Fact]
        public void OrderRequest_TypeInfo_ShouldBePublic()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_HasParameterlessConstructor()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act
            var constructors = type.GetConstructors();
            var parameterlessConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);

            // Assert
            parameterlessConstructor.Should().NotBeNull();
        }
    }
}