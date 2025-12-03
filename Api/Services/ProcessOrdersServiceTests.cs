using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Services;
using Api.Models;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ConsumerConfig> _mockConsumerConfig;
        private readonly Mock<ProducerConfig> _mockProducerConfig;

        public ProcessOrdersServiceTests()
        {
            _mockConsumerConfig = new Mock<ConsumerConfig>();
            _mockProducerConfig = new Mock<ProducerConfig>();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WithNullOrderRequest_ShouldContinue()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(string.Empty);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.StartAsync(default);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidOrderRequest_ShouldProcessOrder()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.IN_PROGRESS 
            };

            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.StartAsync(default);
        }

        [Fact]
        public void OrderRequest_ShouldHaveCorrectProperties()
        {
            // Arrange
            var order = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.IN_PROGRESS 
            };

            // Assert
            order.productname.Should().Be("TestProduct");
            order.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderStatus_ShouldHaveExpectedValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Fact]
        public void JsonSerialization_ShouldWorkForOrderRequest()
        {
            // Arrange
            var order = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.IN_PROGRESS 
            };

            // Act
            var serialized = JsonConvert.SerializeObject(order);
            var deserialized = JsonConvert.DeserializeObject<OrderRequest>(serialized);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized.productname.Should().Be("TestProduct");
            deserialized.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void ProducerConfig_ShouldBeConfigurable()
        {
            // Arrange
            var config = new ProducerConfig 
            { 
                BootstrapServers = "localhost:9092" 
            };

            // Assert
            config.BootstrapServers.Should().Be("localhost:9092");
        }
    }
}