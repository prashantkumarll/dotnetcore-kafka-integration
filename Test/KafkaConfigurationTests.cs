using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Test
{
    public class KafkaConfigurationTests
    {
        [Fact]
        public void ProducerConfig_DefaultConstructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var config = new ProducerConfig();
            
            // Assert
            config.Should().NotBeNull();
            config.Should().BeOfType<ProducerConfig>();
        }
        
        [Fact]
        public void ProducerConfig_BootstrapServers_ShouldSetCorrectly()
        {
            // Arrange
            var config = new ProducerConfig();
            var servers = "localhost:9092,localhost:9093";
            
            // Act
            config.BootstrapServers = servers;
            
            // Assert
            config.BootstrapServers.Should().Be(servers);
        }
        
        [Fact]
        public void ProducerConfig_Acks_ShouldSetToAll()
        {
            // Arrange
            var config = new ProducerConfig();
            
            // Act
            config.Acks = Acks.All;
            
            // Assert
            config.Acks.Should().Be(Acks.All);
        }
        
        [Fact]
        public void ProducerConfig_EnableIdempotence_ShouldSetCorrectly()
        {
            // Arrange
            var config = new ProducerConfig();
            
            // Act
            config.EnableIdempotence = true;
            
            // Assert
            config.EnableIdempotence.Should().BeTrue();
        }
        
        [Fact]
        public void ProducerConfig_CompressionType_ShouldSetCorrectly()
        {
            // Arrange
            var config = new ProducerConfig();
            
            // Act
            config.CompressionType = CompressionType.Snappy;
            
            // Assert
            config.CompressionType.Should().Be(CompressionType.Snappy);
        }
        
        [Fact]
        public void ProducerConfig_MessageTimeoutMs_ShouldSetCorrectly()
        {
            // Arrange
            var config = new ProducerConfig();
            var timeout = 30000;
            
            // Act
            config.MessageTimeoutMs = timeout;
            
            // Assert
            config.MessageTimeoutMs.Should().Be(timeout);
        }
        
        [Fact]
        public void ProducerConfig_MaxInFlight_ShouldSetCorrectly()
        {
            // Arrange
            var config = new ProducerConfig();
            var maxInFlight = 5;
            
            // Act
            config.MaxInFlight = maxInFlight;
            
            // Assert
            config.MaxInFlight.Should().Be(maxInFlight);
        }
        
        [Fact]
        public void ConsumerConfig_DefaultConstructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var config = new ConsumerConfig();
            
            // Assert
            config.Should().NotBeNull();
            config.Should().BeOfType<ConsumerConfig>();
        }
        
        [Fact]
        public void ConsumerConfig_GroupId_ShouldSetCorrectly()
        {
            // Arrange
            var config = new ConsumerConfig();
            var groupId = "test-consumer-group";
            
            // Act
            config.GroupId = groupId;
            
            // Assert
            config.GroupId.Should().Be(groupId);
        }
        
        [Fact]
        public void ConsumerConfig_AutoOffsetReset_ShouldSetToEarliest()
        {
            // Arrange
            var config = new ConsumerConfig();
            
            // Act
            config.AutoOffsetReset = AutoOffsetReset.Earliest;
            
            // Assert
            config.AutoOffsetReset.Should().Be(AutoOffsetReset.Earliest);
        }
        
        [Fact]
        public void ConsumerConfig_AutoOffsetReset_ShouldSetToLatest()
        {
            // Arrange
            var config = new ConsumerConfig();
            
            // Act
            config.AutoOffsetReset = AutoOffsetReset.Latest;
            
            // Assert
            config.AutoOffsetReset.Should().Be(AutoOffsetReset.Latest);
        }
        
        [Fact]
        public void ConsumerConfig_EnableAutoCommit_ShouldSetCorrectly()
        {
            // Arrange
            var config = new ConsumerConfig();
            
            // Act
            config.EnableAutoCommit = false;
            
            // Assert
            config.EnableAutoCommit.Should().BeFalse();
        }
        
        [Fact]
        public void ConsumerConfig_SessionTimeoutMs_ShouldSetCorrectly()
        {
            // Arrange
            var config = new ConsumerConfig();
            var timeout = 10000;
            
            // Act
            config.SessionTimeoutMs = timeout;
            
            // Assert
            config.SessionTimeoutMs.Should().Be(timeout);
        }
        
        [Fact]
        public void ProducerConfig_MultipleProperties_ShouldSetIndependently()
        {
            // Arrange
            var config = new ProducerConfig();
            
            // Act
            config.BootstrapServers = "localhost:9092";
            config.Acks = Acks.Leader;
            config.CompressionType = CompressionType.Gzip;
            
            // Assert
            config.BootstrapServers.Should().Be("localhost:9092");
            config.Acks.Should().Be(Acks.Leader);
            config.CompressionType.Should().Be(CompressionType.Gzip);
        }
        
        [Fact]
        public void ConsumerConfig_MultipleProperties_ShouldSetIndependently()
        {
            // Arrange
            var config = new ConsumerConfig();
            
            // Act
            config.BootstrapServers = "localhost:9092";
            config.GroupId = "test-group";
            config.AutoOffsetReset = AutoOffsetReset.Earliest;
            config.EnableAutoCommit = true;
            
            // Assert
            config.BootstrapServers.Should().Be("localhost:9092");
            config.GroupId.Should().Be("test-group");
            config.AutoOffsetReset.Should().Be(AutoOffsetReset.Earliest);
            config.EnableAutoCommit.Should().BeTrue();
        }
    }
}