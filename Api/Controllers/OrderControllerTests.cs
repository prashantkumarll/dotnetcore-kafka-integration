{
  "testCasesFound": 0,
  "newTestCasesAdded": 8,
  "generatedTestCode": "using System;
using System.Threading.Tasks;
using Xunit;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers;
using Api.Models;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Api.Tests.Controllers
{
    [TestClass]
    public class OrderControllerTests
    {
        private Mock<ProducerConfig> _mockProducerConfig;
        private Mock<ProducerWrapper> _mockProducerWrapper;

        [TestInitialize]
        public void Setup()
        {
            _mockProducerConfig = new Mock<ProducerConfig>();
            _mockProducerWrapper = new Mock<ProducerWrapper>();
        }

        [Fact]
        [TestMethod]
        public async Task PostAsync_ValidOrder_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                CustomerId = 1, 
                ProductId = 100, 
                Quantity = 5 
 ...