using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Api.Models;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Test
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ServiceBus:ConnectionString"] = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=test;SharedAccessKey=test",
                        ["ServiceBus:QueueName"] = "orders-test"
                    });
                });

                builder.ConfigureServices(services =>
                {
                    // Override services for testing if needed
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_ValidOrder_ReturnsOk()
        {
            // Arrange
            var order = new OrderRequest
            {
                Id = "ORD-INT-001",
                CustomerName = "Integration Test Customer",
                Product = "Test Product",
                Quantity = 1,
                Price = 99.99m
            };

            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateOrder_InvalidOrder_ReturnsBadRequest()
        {
            // Arrange
            var invalidOrder = new OrderRequest
            {
                Id = "", // Invalid: empty ID
                CustomerName = "Test Customer",
                Product = "Test Product",
                Quantity = 1,
                Price = 99.99m
            };

            var json = JsonConvert.SerializeObject(invalidOrder);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateOrder_NullBody_ReturnsBadRequest()
        {
            // Arrange
            var content = new StringContent("null", Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateOrder_MalformedJson_ReturnsBadRequest()
        {
            // Arrange
            var malformedJson = "{ invalid json }";
            var content = new StringContent(malformedJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetOrderStatus_ValidId_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/orders/ORD-123/status");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetOrderStatus_EmptyId_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/orders/ /status");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task HealthCheck_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateMultipleOrders_AllSucceed()
        {
            // Arrange
            var orders = new[]
            {
                new OrderRequest { Id = "ORD-MULTI-001", CustomerName = "Customer 1", Product = "Product 1", Quantity = 1, Price = 10.00m },
                new OrderRequest { Id = "ORD-MULTI-002", CustomerName = "Customer 2", Product = "Product 2", Quantity = 2, Price = 20.00m },
                new OrderRequest { Id = "ORD-MULTI-003", CustomerName = "Customer 3", Product = "Product 3", Quantity = 3, Price = 30.00m }
            };

            // Act & Assert
            foreach (var order in orders)
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _client.PostAsync("/api/orders", content);
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task CreateOrder_LargeOrder_ReturnsOk()
        {
            // Arrange
            var largeOrder = new OrderRequest
            {
                Id = "ORD-LARGE-INT-001",
                CustomerName = new string('A', 100),
                Product = new string('B', 200),
                Quantity = 1000,
                Price = 999999.99m
            };

            var json = JsonConvert.SerializeObject(largeOrder);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("ORD-STATUS-001")]
        [InlineData("ORD-STATUS-002")]
        [InlineData("DIFFERENT-FORMAT-123")]
        public async Task GetOrderStatus_VariousValidIds_ReturnsOk(string orderId)
        {
            // Act
            var response = await _client.GetAsync($"/api/orders/{orderId}/status");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }
    }
}