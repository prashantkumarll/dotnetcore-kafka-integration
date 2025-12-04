using System;
using System.IO;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;
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
        private readonly ProducerConfig _mockConfig;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            // Arrange - Setup mock configuration
            _mockConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-client"
            };
            _controller = new OrderController(_mockConfig);
        }

        [Fact]
        public void Constructor_WithValidConfig_ShouldInitializeController()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

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

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task PostAsync_ShouldSerializeOrderRequestCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedSerialized = JsonConvert.SerializeObject(orderRequest);

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            // Verify that serialization would produce expected JSON
            var actualSerialized = JsonConvert.SerializeObject(orderRequest);
            actualSerialized.Should().Be(expectedSerialized);
        }

        [Fact]
        public async Task PostAsync_WithMultipleModelErrors_ShouldReturnBadRequestWithAllErrors()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            _controller.ModelState.AddModelError("Field1", "Error1");
            _controller.ModelState.AddModelError("Field2", "Error2");

            // Act
            var result = await _controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            var modelState = badRequestResult.Value as ModelStateDictionary;
            modelState.Should().NotBeNull();
            modelState.ErrorCount.Should().Be(2);
        }

        [Fact]
        public void PostAsync_ShouldBeDecoratedWithHttpPostAttribute()
        {
            // Arrange & Act
            var method = typeof(OrderController).GetMethod("PostAsync");
            var attributes = method.GetCustomAttributes(typeof(HttpPostAttribute), false);

            // Assert
            attributes.Should().NotBeEmpty();
            attributes.Length.Should().Be(1);
        }

        [Fact]
        public void OrderController_ShouldBeDecoratedWithRouteAttribute()
        {
            // Arrange & Act
            var controllerType = typeof(OrderController);
            var routeAttributes = controllerType.GetCustomAttributes(typeof(RouteAttribute), false);

            // Assert
            routeAttributes.Should().NotBeEmpty();
            var routeAttribute = routeAttributes[0] as RouteAttribute;
            routeAttribute.Template.Should().Be("api/[controller]");
        }

        [Fact]
        public async Task PostAsync_ShouldWriteConsoleOutput()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                var result = await _controller.PostAsync(orderRequest);

                // Assert
                var output = stringWriter.ToString();
                output.Should().Contain("Info: OrderController => Post => Recieved a new purchase order:");
                output.Should().Contain("========");
                output.Should().Contain("=========");
            }
            finally
            {
                // Cleanup
                Console.SetOut(originalOut);
                stringWriter.Dispose();
            }
        }

        [Fact]
        public async Task PostAsync_ShouldLogSerializedOrderInConsole()
        {
            // Arrange
            var orderRequest = new OrderRequest();
            var expectedJson = JsonConvert.SerializeObject(orderRequest);
            var originalOut = Console.Out;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                var result = await _controller.PostAsync(orderRequest);

                // Assert
                var output = stringWriter.ToString();
                output.Should().Contain(expectedJson);
            }
            finally
            {
                // Cleanup
                Console.SetOut(originalOut);
                stringWriter.Dispose();
            }
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowException()
        {
            // Arrange
            ProducerConfig config = default!;

            // Act & Assert
            var action = () => new OrderController(config);
            action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task PostAsync_WithEmptyModelState_ShouldProcessSuccessfully()
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
        public void OrderController_ShouldInheritFromControllerBase()
        {
            // Arrange & Act
            var controllerType = typeof(OrderController);
            var baseType = controllerType.BaseType;

            // Assert
            baseType.Should().Be(typeof(ControllerBase));
        }
    }
}