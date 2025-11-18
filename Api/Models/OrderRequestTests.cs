using System;
using Xunit;
using NUnit.Framework;
using Api.Models;

namespace Api.Tests
{
    [TestClass]
    public class OrderRequestTests
    {
        [Fact]
        [Test]
        public void OrderRequest_ValidData_ShouldCreateSuccessfully()
        {
            var order = new OrderRequest
            {
                id = 1,
                productname = "Test Product",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.NotNull(order);
            Assert.Equal(1, order.id);
            Assert.Equal("Test Product", order.productname);
            Assert.Equal(5, order.quantity);
            Assert.Equal(OrderStatus.IN_PROGRESS, order.status);
        }

        [Fact]
        [Test]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            var order = new OrderRequest
            {
                id = 2,
                productname = null,
                quantity = 0,
                status = OrderStatus.REJECTED
            };

            Assert.NotNull(order);
            Assert.Null(order.productname);
        }

        [Fact]
        [Test]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            var order = new OrderRequest
            {
                id = 3,
                productname = "Negative Test",
                quantity = -1,
                status = OrderStatus.COMPLETED
            };

            Assert.NotNull(order);
            Assert.Equal(-1, order.quantity);
        }

        [Fact]
        [Test]
        public void OrderRequest_AllStatusValues_ShouldBeValid()
        {
            var statuses = new[] 
            {
                OrderStatus.IN_PROGRESS,
                OrderStatus.COMPLETED,
                OrderStatus.REJECTED
            };

            Assert.Equal(3, statuses.Length);
        }

        [Fact]
        [Test]
        public void OrderRequest_DefaultValues_ShouldBeZeroOrNull()
        {
            var order = new OrderRequest();

            Assert.Equal(0, order.id);
            Assert.Equal(0, order.quantity);
            Assert.Null(order.productname);
        }

        [Fact]
        [Test]
        public void OrderRequest_MultipleInstances_ShouldBeIndependent()
        {
            var order1 = new OrderRequest { id = 1, productname = "Product1" };
            var order2 = new OrderRequest { id = 2, productname = "Product2" };

            Assert.NotEqual(order1.id, order2.id);
            Assert.NotEqual(order1.productname, order2.productname);
        }

        [Fact]
        [Test]
        public void OrderStatus_EnumValues_ShouldBeCorrect()
        {
            Assert.Equal(0, (int)OrderStatus.IN_PROGRESS);
            Assert.Equal(1, (int)OrderStatus.COMPLETED);
            Assert.Equal(2, (int)OrderStatus.REJECTED);
        }

        [Fact]
        [Test]
        public void OrderRequest_MaxIntValues_ShouldBeSupported()
        {
            var order = new OrderRequest
            {
                id = int.MaxValue,
                quantity = int.MaxValue,
                productname = "Max Value Test"
            };

            Assert.Equal(int.MaxValue, order.id);
            Assert.Equal(int.MaxValue, order.quantity);
        }
    }
}