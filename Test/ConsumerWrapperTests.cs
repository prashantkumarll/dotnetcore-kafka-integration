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
using Api;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Constructor_WithValidConfigAndTopic_ShouldCreateInstance()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            var topicName = "test-topic";
            
            // Act
            var wrapper = new ConsumerWrapper(config, topicName);
            
            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Should().BeOfType<ConsumerWrapper>();
        }
        
        [Fact]
        public void ConsumerWrapper_Constructor_WithNullConfig_ShouldThrow()
        {
            // Arrange
            ConsumerConfig config = null;
            var topicName = "test-topic";
            
            // Act & Assert
            var action = () => new ConsumerWrapper(config, topicName);
            action.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void ConsumerWrapper_Constructor_WithNullTopic_ShouldThrow()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            string topicName = null;
            
            // Act & Assert
            var action = () => new ConsumerWrapper(config, topicName);
            action.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void ConsumerWrapper_Constructor_WithEmptyTopic_ShouldThrow()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            var topicName = "";
            
            // Act & Assert
            var action = () => new ConsumerWrapper(config, topicName);
            action.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void ConsumerWrapper_ReadMessageMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("readMessage");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
        
        [Fact]
        public void ConsumerWrapper_DisposeMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("Dispose");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
        
        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }
        
        [Fact]
        public void ConsumerWrapper_Constructor_WithDifferentOffsetReset_ShouldSucceed()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "orders-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            var topicName = "orders-topic";
            
            // Act
            var wrapper = new ConsumerWrapper(config, topicName);
            
            // Assert
            wrapper.Should().NotBeNull();
        }
        
        [Fact]
        public void ConsumerWrapper_Constructor_WithCommitConfig_ShouldSucceed()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "events-group",
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            var topicName = "events-topic";
            
            // Act
            var wrapper = new ConsumerWrapper(config, topicName);
            
            // Assert
            wrapper.Should().NotBeNull();
        }
        
        [Fact]
        public void ConsumerWrapper_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var config1 = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "group-1"
            };
            var config2 = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "group-2"
            };
            var topicName = "test-topic";
            
            // Act
            var wrapper1 = new ConsumerWrapper(config1, topicName);
            var wrapper2 = new ConsumerWrapper(config2, topicName);
            
            // Assert
            wrapper1.Should().NotBeSameAs(wrapper2);
        }
        
        [Fact]
        public void ConsumerWrapper_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }
        
        [Fact]
        public void ConsumerWrapper_GetType_ShouldReturnCorrectType()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            var wrapper = new ConsumerWrapper(config, "test-topic");
            
            // Act
            var type = wrapper.GetType();
            
            // Assert
            type.Should().Be(typeof(ConsumerWrapper));
        }
    }
}