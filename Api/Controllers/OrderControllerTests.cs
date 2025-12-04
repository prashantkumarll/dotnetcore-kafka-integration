using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using Confluent.Kafka;
using Api.Controllers;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task PostAsync_ValidOrderRequest_ReturnsCreatedResult()
        {
            // Arrange
            var mockConfig = new ProducerConfig();
            var controller = new OrderController(mockConfig);
            var orderRequest = new OrderRequest
            {
                // Populate with minimal valid data based on OrderRequest structure
            };

            // Act
            var result = await controller.PostAsync(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedResult>(); 
        }
    }
}