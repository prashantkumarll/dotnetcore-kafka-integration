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
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Constructor_WithValidConfigAndTopic_ShouldCreateInstance()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            var topicName = "test-topic";
            
            // Act
            var wrapper = new ProducerWrapper(config, topicName);
            
            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Should().BeOfType<ProducerWrapper>();
        }
        
        [Fact]
        public void ProducerWrapper_Constructor_WithNullConfig_ShouldThrow()
        {
            // Arrange
            ProducerConfig config = null;
            var topicName = "test-topic";
            
            // Act & Assert
            var action = () => new ProducerWrapper(config, topicName);
            action.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void ProducerWrapper_Constructor_WithNullTopic_ShouldThrow()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            string topicName = null;
            
            // Act & Assert
            var action = () => new ProducerWrapper(config, topicName);
            action.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void ProducerWrapper_Constructor_WithEmptyTopic_ShouldThrow()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            var topicName = "";
            
            // Act & Assert
            var action = () => new ProducerWrapper(config, topicName);
            action.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void ProducerWrapper_WriteMessageMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var method = type.GetMethod("writeMessage");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
        
        [Fact]
        public void ProducerWrapper_DisposeMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var method = type.GetMethod("Dispose");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
        
        [Fact]
        public void ProducerWrapper_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }
        
        [Fact]
        public void ProducerWrapper_Constructor_WithDifferentAcks_ShouldSucceed()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All
            };
            var topicName = "orders-topic";
            
            // Act
            var wrapper = new ProducerWrapper(config, topicName);
            
            // Assert
            wrapper.Should().NotBeNull();
        }
        
        [Fact]
        public void ProducerWrapper_Constructor_WithCompressionConfig_ShouldSucceed()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                CompressionType = CompressionType.Gzip,
                EnableIdempotence = true
            };
            var topicName = "events-topic";
            
            // Act
            var wrapper = new ProducerWrapper(config, topicName);
            
            // Assert
            wrapper.Should().NotBeNull();
        }
        
        [Fact]
        public void ProducerWrapper_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topic1 = "topic-1";
            var topic2 = "topic-2";
            
            // Act
            var wrapper1 = new ProducerWrapper(config, topic1);
            var wrapper2 = new ProducerWrapper(config, topic2);
            
            // Assert
            wrapper1.Should().NotBeSameAs(wrapper2);
        }
        
        [Fact]
        public void ProducerWrapper_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }
        
        [Fact]
        public void ProducerWrapper_GetType_ShouldReturnCorrectType()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var wrapper = new ProducerWrapper(config, "test-topic");
            
            // Act
            var type = wrapper.GetType();
            
            // Assert
            type.Should().Be(typeof(ProducerWrapper));
        }
    }
}