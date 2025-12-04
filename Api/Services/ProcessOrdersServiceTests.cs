using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Api.Services;
using Api.Models;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Api.Tests.Services
{
    public class FakeConsumerWrapper : IDisposable
    {
        public string MessageToReturn { get; set; } = string.Empty;
        
        public string readMessage()
        {
            return MessageToReturn;
        }
        
        public void Dispose()
        {
        }
    }
    
    public class FakeProducerWrapper : IDisposable
    {
        public string LastWrittenMessage { get; private set; } = string.Empty;
        
        public async Task writeMessage(string message)
        {
            LastWrittenMessage = message;
            await Task.CompletedTask;
        }
        
        public void Dispose()
        {
        }
    }
    
    public class TestableProcessOrdersService : ProcessOrdersService
    {
        public FakeConsumerWrapper FakeConsumer { get; set; } = new FakeConsumerWrapper();
        public FakeProducerWrapper FakeProducer { get; set; } = new FakeProducerWrapper();
        
        public TestableProcessOrdersService(ConsumerConfig consumerConfig, ProducerConfig producerConfig) 
            : base(consumerConfig, producerConfig)
        {
        }
        
        public async Task TestExecuteAsync(CancellationToken cancellationToken)
        {
            await base.ExecuteAsync(cancellationToken);
        }
    }
    
    public class ProcessOrdersServiceTests
    {
        private readonly ConsumerConfig consumerConfig;
        private readonly ProducerConfig producerConfig;
        
        public ProcessOrdersServiceTests()
        {
            consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
        }
        
        [Fact]
        public void Constructor_WithValidConfigs_ShouldCreateInstance()
        {
            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);
            
            // Assert
            service.Should().NotBeNull();
        }
        
        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldNotThrow()
        {
            // Act & Assert
            var action = () => new ProcessOrdersService(null, producerConfig);
            action.Should().NotThrow();
        }
        
        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldNotThrow()
        {
            // Act & Assert
            var action = () => new ProcessOrdersService(consumerConfig, null);
            action.Should().NotThrow();
        }
        
        [Fact]
        public void OrderStatus_Enum_ShouldHaveExpectedValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();
            
            // Assert
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Length.Should().BeGreaterThan(0);
        }
        
        [Fact]
        public void OrderRequest_Serialization_ShouldWorkCorrectly()
        {
            // Arrange
            var order = new OrderRequest
            {
                productname = "Test Product",
                status = OrderStatus.COMPLETED
            };
            
            // Act
            var json = JsonConvert.SerializeObject(order);
            var deserializedOrder = JsonConvert.DeserializeObject<OrderRequest>(json);
            
            // Assert
            json.Should().NotBeNullOrEmpty();
            deserializedOrder.Should().NotBeNull();
            deserializedOrder.productname.Should().Be("Test Product");
            deserializedOrder.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}