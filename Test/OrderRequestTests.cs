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
            // Act
            var order = new OrderRequest();
            
            // Assert
            order.Should().NotBeNull();
            order.Should().BeOfType<OrderRequest>();
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
            // Act
            var order1 = new OrderRequest();
            var order2 = new OrderRequest();
            
            // Assert
            order1.Should().NotBeSameAs(order2);
        }

        [Fact]
        public void OrderRequest_TypeProperties_ShouldBeAccessible()
        {
            // Arrange
            var type = typeof(OrderRequest);
            
            // Act
            var constructors = type.GetConstructors();
            var properties = type.GetProperties();
            
            // Assert
            constructors.Should().NotBeEmpty();
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
        }
    }
}