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
            // Arrange & Act
            var order1 = new OrderRequest();
            var order2 = new OrderRequest();

            // Assert
            order1.Should().NotBeSameAs(order2);
            order1.Should().NotBeNull();
            order2.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_TypeInfo_ShouldHavePublicConstructor()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act
            var constructors = type.GetConstructors();

            // Assert
            constructors.Should().NotBeEmpty();
            constructors.Should().Contain(c => c.IsPublic);
        }

        [Fact]
        public void OrderRequest_Assembly_ShouldBeCorrect()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act
            var assembly = type.Assembly;

            // Assert
            assembly.Should().NotBeNull();
            assembly.GetName().Name.Should().Be("Api");
        }
    }
}