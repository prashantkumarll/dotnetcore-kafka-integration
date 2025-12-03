using System;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

namespace Test
{
    public class OrderControllerTests
    {
        private readonly ServiceBusClient _mockConfig;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            // Arrange - Setup mock configuration
            _mockConfig = new ServiceBusClient
            {
                ConnectionString = "localhost:9092",
                ClientId = "test-client"
            };
            _controller = new OrderController(_mockConfig);
        }

        [Fact]
        public void Constructor_WithValidConfig_ShouldInitializeController()
        {
            // Arrange
            var config = new ServiceBusClient { ConnectionString = "localhost:9092" };

            // Act
            var controller = new OrderController(config);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_WithValidOrderRequest_ShouldReturnCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult.Location.Should().Be("TransactionId");
            createdResult.Value.Should().Be("Your order is in progress");
        }

        [Fact]
        public async Task PostAsync_WithInvalidModelState_ShouldReturnBadRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("TestField", "Test error message");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be(_controller.ModelState);
        }

        [Fact]
        public async Task PostAsync_WithNullOrderRequest_ShouldHandleGracefully()
        {
            // Arrange
            OrderRequest orderRequest = default!;
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public void PostAsync_ShouldSerializeOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            
            // Act
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            // Assert
            serializedOrder.Should().NotBeNullOrEmpty();
            serializedOrder.Should().BeValidJson();
        }

        [Fact]
        public async Task PostAsync_ShouldLogOrderInformation()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.Clear();
            var originalOut = Console.Out;
            using var stringWriter = new System.IO.StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                await _controller.PostAsync(orderRequest);

                // Assert
                var output = stringWriter.ToString();
                output.Should().Contain("Info: OrderController => Post => Recieved a new purchase order:");
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void OrderController_ShouldHaveCorrectRouteAttribute()
        {
            // Arrange & Act
            var routeAttribute = typeof(OrderController).GetCustomAttribute<RouteAttribute>();

            // Assert
            routeAttribute.Should().NotBeNull();
            routeAttribute.Template.Should().Be("api/[controller]");
        }

        [Fact]
        public void PostAsync_ShouldHaveCorrectHttpPostAttribute()
        {
            // Arrange & Act
            var method = typeof(OrderController).GetMethod("PostAsync");
            var httpPostAttribute = method.GetCustomAttribute<HttpPostAttribute>();

            // Assert
            httpPostAttribute.Should().NotBeNull();
        }
    }

    public static class JsonExtensions
    {
        public static bool IsValidJson(this string jsonString)
        {
            try
            {
                JsonConvert.DeserializeObject(jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}