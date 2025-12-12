using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        public void OrderRequest_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("OrderRequest");
            type.Namespace.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var order1 = new OrderRequest();
            var order2 = new OrderRequest();

            // Act & Assert
            order1.Should().NotBeNull();
            order2.Should().NotBeNull();
            order1.Should().NotBeSameAs(order2);
        }

        [Fact]
        public void OrderRequest_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
        }
    }
}