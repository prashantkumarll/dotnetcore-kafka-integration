using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Api.Services;
using Confluent.Kafka;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Constructor_ShouldAcceptConfigurations()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            
            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);
            
            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }
        
        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProcessOrdersService");
            type.Namespace.Should().Be("Api.Services");
        }
        
        [Fact]
        public void ProcessOrdersService_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            
            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
        }
    }
}