using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using Moq;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Services;
using Api.Models;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig();
            var producerConfig = new ProducerConfig();

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void OrderStatus_ShouldHaveExpectedValues()
        {
            // Arrange & Act
            var orderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            orderStatuses.Should().Contain(OrderStatus.IN_PROGRESS);
            orderStatuses.Should().Contain(OrderStatus.COMPLETED);
            orderStatuses.Should().Contain(OrderStatus.REJECTED);
        }

        [Fact]
        public void OrderRequest_ShouldHaveRequiredProperties()
        {
            // Arrange
            var order = new OrderRequest
            {
                productname = "Test Product",
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            order.productname.Should().Be("Test Product");
            order.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void Constructor_ShouldAcceptNullConfigs()
        {
            // Arrange
            ConsumerConfig consumerConfig = null;
            ProducerConfig producerConfig = null;

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void OrderRequest_ShouldAllowStatusChange()
        {
            // Arrange
            var order = new OrderRequest
            {
                productname = "Sample Product",
                status = OrderStatus.IN_PROGRESS
            };

            // Act
            order.status = OrderStatus.COMPLETED;

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
            order.productname.Should().Be("Sample Product");
        }
    }
}