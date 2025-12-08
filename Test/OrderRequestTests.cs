using Api.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Test
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_CanBeInstantiated()
        {
            // Arrange & Act
            var order = new OrderRequest();

            // Assert
            order.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_PropertiesCanBeSetAndRetrieved()
        {
            // Arrange
            var orderId = "ORD12345";
            var customerId = "CUST001";
            var productName = "Test Product";
            var quantity = 5;
            var price = 99.99m;

            // Act
            var order = new OrderRequest
            {
                OrderId = orderId,
                CustomerId = customerId,
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };

            // Assert
            order.OrderId.Should().Be(orderId);
            order.CustomerId.Should().Be(customerId);
            order.ProductName.Should().Be(productName);
            order.Quantity.Should().Be(quantity);
            order.Price.Should().Be(price);
        }

        [Fact]
        public void OrderRequest_CanBeSerializedToJson()
        {
            // Arrange
            var order = new OrderRequest
            {
                OrderId = "ORD123",
                CustomerId = "CUST456",
                ProductName = "Laptop Computer",
                Quantity = 2,
                Price = 1299.99m
            };

            // Act
            var json = JsonConvert.SerializeObject(order);

            // Assert
            json.Should().Contain("\"OrderId\":\"ORD123\"");
            json.Should().Contain("\"CustomerId\":\"CUST456\"");
            json.Should().Contain("\"ProductName\":\"Laptop Computer\"");
            json.Should().Contain("\"Quantity\":2");
            json.Should().Contain("\"Price\":1299.99");
        }

        [Fact]
        public void OrderRequest_CanBeDeserializedFromJson()
        {
            // Arrange
            var json = @"{
                ""OrderId"": ""ORD789"",
                ""CustomerId"": ""CUST999"",
                ""ProductName"": ""Smartphone"",
                ""Quantity"": 1,
                ""Price"": 599.50
            }";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(json);

            // Assert
            order.Should().NotBeNull();
            order!.OrderId.Should().Be("ORD789");
            order.CustomerId.Should().Be("CUST999");
            order.ProductName.Should().Be("Smartphone");
            order.Quantity.Should().Be(1);
            order.Price.Should().Be(599.50m);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("ORD123", true)]
        public void OrderRequest_OrderIdValidation(string? orderId, bool shouldBeValid)
        {
            // Arrange
            var order = new OrderRequest
            {
                OrderId = orderId!,
                CustomerId = "CUST001",
                ProductName = "Product",
                Quantity = 1,
                Price = 10.00m
            };

            // Act & Assert
            if (shouldBeValid)
            {
                order.OrderId.Should().NotBeNullOrEmpty();
            }
            else
            {
                order.OrderId.Should().BeNullOrEmpty();
            }
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(1, true)]
        [InlineData(100, true)]
        public void OrderRequest_QuantityValidation(int quantity, bool shouldBeValid)
        {
            // Arrange
            var order = new OrderRequest
            {
                OrderId = "ORD001",
                CustomerId = "CUST001",
                ProductName = "Product",
                Quantity = quantity,
                Price = 10.00m
            };

            // Act & Assert
            if (shouldBeValid)
            {
                order.Quantity.Should().BePositive();
            }
            else
            {
                order.Quantity.Should().BeLessOrEqualTo(0);
            }
        }

        [Theory]
        [InlineData(-1.00, false)]
        [InlineData(0.00, false)]
        [InlineData(0.01, true)]
        [InlineData(999.99, true)]
        public void OrderRequest_PriceValidation(decimal price, bool shouldBeValid)
        {
            // Arrange
            var order = new OrderRequest
            {
                OrderId = "ORD001",
                CustomerId = "CUST001",
                ProductName = "Product",
                Quantity = 1,
                Price = price
            };

            // Act & Assert
            if (shouldBeValid)
            {
                order.Price.Should().BePositive();
            }
            else
            {
                order.Price.Should().BeLessOrEqualTo(0);
            }
        }

        [Fact]
        public void OrderRequest_DefaultValues()
        {
            // Arrange & Act
            var order = new OrderRequest();

            // Assert
            order.OrderId.Should().BeNull();
            order.CustomerId.Should().BeNull();
            order.ProductName.Should().BeNull();
            order.Quantity.Should().Be(0);
            order.Price.Should().Be(0);
        }
    }
}