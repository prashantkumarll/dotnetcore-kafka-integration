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
        public void OrderRequest_DefaultConstructor_ShouldCreateInstance()
        {
            // Act
            var order = new OrderRequest();

            // Assert
            order.Should().NotBeNull();
        }

        [Theory]
        [InlineData("ORDER-001")]
        [InlineData("12345")]
        [InlineData("")]
        [InlineData(null)]
        public void OrderRequest_OrderIdProperty_ShouldSetAndGetCorrectly(string orderId)
        {
            // Arrange
            var order = new OrderRequest();

            // Act
            order.OrderId = orderId;

            // Assert
            order.OrderId.Should().Be(orderId);
        }

        [Theory]
        [InlineData("John Doe")]
        [InlineData("Jane Smith")]
        [InlineData("")]
        [InlineData(null)]
        public void OrderRequest_CustomerNameProperty_ShouldSetAndGetCorrectly(string customerName)
        {
            // Arrange
            var order = new OrderRequest();

            // Act
            order.CustomerName = customerName;

            // Assert
            order.CustomerName.Should().Be(customerName);
        }

        [Fact]
        public void OrderRequest_WithMultipleProperties_ShouldMaintainValues()
        {
            // Arrange & Act
            var order = new OrderRequest
            {
                OrderId = "ORDER-123",
                CustomerName = "Test Customer"
            };

            // Assert
            order.OrderId.Should().Be("ORDER-123");
            order.CustomerName.Should().Be("Test Customer");
        }

        [Fact]
        public void OrderRequest_Type_ShouldHaveExpectedProperties()
        {
            // Arrange
            var orderType = typeof(OrderRequest);

            // Act
            var properties = orderType.GetProperties();

            // Assert
            properties.Should().NotBeEmpty();
            properties.Should().Contain(p => p.Name == "OrderId");
            properties.Should().Contain(p => p.Name == "CustomerName");
        }

        [Theory]
        [InlineData("ORDER-001", "Customer 1")]
        [InlineData("ORDER-002", "Customer 2")]
        [InlineData("", "")]
        public void OrderRequest_ObjectInitializer_ShouldSetProperties(string orderId, string customerName)
        {
            // Act
            var order = new OrderRequest
            {
                OrderId = orderId,
                CustomerName = customerName
            };

            // Assert
            order.OrderId.Should().Be(orderId);
            order.CustomerName.Should().Be(customerName);
        }

        [Fact]
        public void OrderRequest_Properties_ShouldBeReadWrite()
        {
            // Arrange
            var orderType = typeof(OrderRequest);
            var orderIdProperty = orderType.GetProperty("OrderId");
            var customerNameProperty = orderType.GetProperty("CustomerName");

            // Assert
            orderIdProperty?.CanRead.Should().BeTrue();
            orderIdProperty?.CanWrite.Should().BeTrue();
            customerNameProperty?.CanRead.Should().BeTrue();
            customerNameProperty?.CanWrite.Should().BeTrue();
        }
    }
}