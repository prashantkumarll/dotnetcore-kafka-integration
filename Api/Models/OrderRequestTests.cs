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
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.NotNull(order);
            Assert.Equal(1, order.id);
            Assert.Equal("Test Product", order.productname);
            Assert.Equal(10, order.quantity);
            Assert.Equal(OrderStatus.IN_PROGRESS, order.status);
        }

        [Fact]
        [Test]
        public void OrderRequest_NullProductName_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => {
                var order = new OrderRequest
                {
                    id = 1,
                    productname = null,
                    quantity = 10,
                    status = OrderStatus.IN_PROGRESS
                };
            });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [Test]
        public void OrderRequest_InvalidQuantity_ShouldThrowArgumentException(int invalidQuantity)
        {
            Assert.Throws<ArgumentException>(() => {
                var order = new OrderRequest
                {
                    id = 1,
                    productname = "Test Product",
                    quantity = invalidQuantity,
                    status = OrderStatus.IN_PROGRESS
                };
            });
        }

        [Fact]
        [Test]
        public void OrderRequest_AllOrderStatuses_ShouldBeValid()
        {
            var statuses = new[] 
            {
                OrderStatus.IN_PROGRESS,
                OrderStatus.COMPLETED,
                OrderStatus.REJECTED
            };

            foreach (var status in statuses)
            {
                var order = new OrderRequest
                {
                    id = 1,
                    productname = "Test Product",
                    quantity = 10,
                    status = status
                };

                Assert.Equal(status, order.status);
            }
        }

        [Fact]
        [Test]
        public void OrderRequest_MaxIntQuantity_ShouldBeAccepted()
        {
            var order = new OrderRequest
            {
                id = 1,
                productname = "Test Product",
                quantity = int.MaxValue,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.Equal(int.MaxValue, order.quantity);
        }

        [Fact]
        [Test]
        public void OrderRequest_EmptyProductName_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => {
                var order = new OrderRequest
                {
                    id = 1,
                    productname = string.Empty,
                    quantity = 10,
                    status = OrderStatus.IN_PROGRESS
                };
            });
        }

        [Fact]
        [Test]
        public void OrderRequest_DefaultConstructor_ShouldInitializeProperties()
        {
            var order = new OrderRequest();

            Assert.Equal(0, order.id);
            Assert.Null(order.productname);
            Assert.Equal(0, order.quantity);
            Assert.Equal(default(OrderStatus), order.status);
        }
    }
}