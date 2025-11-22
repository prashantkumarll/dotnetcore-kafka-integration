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
                productname = "TestProduct",
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.NotNull(order);
            Assert.Equal(1, order.id);
            Assert.Equal("TestProduct", order.productname);
            Assert.Equal(10, order.quantity);
            Assert.Equal(OrderStatus.IN_PROGRESS, order.status);
        }

        [Fact]
        [Test]
        public void OrderRequest_NullProductName_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => {
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
                    productname = "TestProduct",
                    quantity = invalidQuantity,
                    status = OrderStatus.IN_PROGRESS
                };
            });
        }

        [Fact]
        [Test]
        public void OrderRequest_AllStatusValues_ShouldBeValid()
        {
            var statuses = new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED };
            foreach (var status in statuses)
            {
                var order = new OrderRequest
                {
                    id = 1,
                    productname = "TestProduct",
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
                productname = "TestProduct",
                quantity = int.MaxValue,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.Equal(int.MaxValue, order.quantity);
        }

        [Fact]
        [Test]
        public void OrderRequest_LongProductName_ShouldBeAccepted()
        {
            var longProductName = new string('A', 1000);
            var order = new OrderRequest
            {
                id = 1,
                productname = longProductName,
                quantity = 10,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.Equal(longProductName, order.productname);
        }
    }
}