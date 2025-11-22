{
  "testCasesFound": 0,
  "newTestCasesAdded": 8,
  "packageInstallCommands": [
    "dotnet add package Microsoft.NET.Test.Sdk --version 17.8.0",
    "dotnet add package xunit --version 2.6.2", 
    "dotnet add package xunit.runner.visualstudio --version 2.5.3",
    "dotnet add package Moq --version 4.20.69",
    "dotnet add package FluentAssertions --version 6.12.0",
    "dotnet add package Confluent.Kafka --version 2.3.0"
  ],
  "generatedTestCode": "using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Api.Models;
using Api.Services;

namespace Api.Tests.Services
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ConsumerConfig> _mockConsumerConfig;
        private readonly Mock<ProducerConfig> _mockProducerConfig;
        private readonly Mock<ConsumerWrapper> _mockConsumerWrapper;
        private readonly Mock<ProducerWrapper> _mockProduc...