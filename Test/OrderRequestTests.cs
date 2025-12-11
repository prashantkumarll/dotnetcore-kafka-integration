using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using FluentAssertions;
using FluentAssertions.Collections;
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
        public void OrderRequest_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            isPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderRequest_Type_ShouldNotBeAbstract()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act
            var isAbstract = type.IsAbstract;

            // Assert
            isAbstract.Should().BeFalse();
        }

        [Fact]
        public void OrderRequest_Type_ShouldNotBeInterface()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act
            var isInterface = type.IsInterface;

            // Assert
            isInterface.Should().BeFalse();
        }

        [Fact]
        public void OrderRequest_Constructor_ShouldBeParameterless()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act
            var constructors = type.GetConstructors();
            var parameterlessConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);

            // Assert
            constructors.Should().NotBeNull();
            parameterlessConstructor.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_Properties_ShouldBeAccessible()
        {
            // Arrange
            var type = typeof(OrderRequest);

            // Act
            var properties = type.GetProperties();

            // Assert
            properties.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_Multiple_Instances_ShouldBeIndependent()
        {
            // Arrange & Act
            var orderRequest1 = new OrderRequest();
            var orderRequest2 = new OrderRequest();

            // Assert
            orderRequest1.Should().NotBeSameAs(orderRequest2);
            orderRequest1.Should().NotBeEquivalentTo(orderRequest2, options => options.ComparingByMembers<OrderRequest>());
        }
    }
}