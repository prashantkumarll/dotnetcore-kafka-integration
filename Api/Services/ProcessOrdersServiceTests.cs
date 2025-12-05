using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Api.Services;
using Api.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Api.Tests.Services
{
    public class FakeConsumerWrapper : IDisposable
    {
        private readonly Queue<string> _messages;
        
        public FakeConsumerWrapper(ConsumerConfig config, string topic)
        {
            _messages = new Queue<string>();
        }
        
        public void AddMessage(string message)
        {
            _messages.Enqueue(message);
        }
        
        public string readMessage()
        {
            return _messages.Count > 0 ? _messages.Dequeue() : null;
        }
        
        public void Dispose()
        {
        }
    }
    
    public class FakeProducerWrapper : IDisposable
    {
        public List<string> SentMessages { get; } = new List<string>();
        
        public FakeProducerWrapper(ProducerConfig config, string topic)
        {
        }
        
        public async Task writeMessage(string message)
        {
            SentMessages.Add(message);
            await Task.CompletedTask;
        }
        
        public void Dispose()
        {
        }
    }
    
    public class ProcessOrdersServiceTests
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;
        
        public ProcessOrdersServiceTests()
        {
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
        }
        
        [Fact]
        public void Constructor_WithValidConfigs_ShouldCreateInstance()
        {
            // Act
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            
            // Assert
            service.Should().NotBeNull();
        }
        
        [Fact]
        public async Task StartAsync_ShouldStartSuccessfully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = new CancellationToken();
            
            // Act
            var startTask = service.StartAsync(cancellationToken);
            
            // Assert
            await startTask;
            startTask.IsCompletedSuccessfully.Should().BeTrue();
        }
        
        [Fact]
        public async Task StopAsync_ShouldStopSuccessfully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = new CancellationToken();
            
            // Act
            await service.StartAsync(cancellationToken);
            var stopTask = service.StopAsync(cancellationToken);
            
            // Assert
            await stopTask;
            stopTask.IsCompletedSuccessfully.Should().BeTrue();
        }
        
        [Fact]
        public async Task StartAsync_WithCancellation_ShouldHandleCancellation()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationTokenSource = new CancellationTokenSource();
            
            // Act
            var startTask = service.StartAsync(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();
            
            // Assert
            await startTask;
            startTask.IsCompletedSuccessfully.Should().BeTrue();
        }
        
        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldThrow()
        {
            // Act & Assert
            Action act = () => new ProcessOrdersService(null, _producerConfig);
            act.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrow()
        {
            // Act & Assert
            Action act = () => new ProcessOrdersService(_consumerConfig, null);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}