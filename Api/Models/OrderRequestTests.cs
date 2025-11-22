{
  "testCasesFound": 0,
  "newTestCasesAdded": 8,
  "packageInstallCommands": [
    "dotnet add package Microsoft.NET.Test.Sdk --version 17.8.0",
    "dotnet add package xunit --version 2.6.2", 
    "dotnet add package xunit.runner.visualstudio --version 2.5.3",
    "dotnet add package Moq --version 4.20.69",
    "dotnet add package FluentAssertions --version 6.12.0"
  ],
  "generatedTestCode": "using System;
using Xunit;
using FluentAssertions;

namespace Api.Models.Tests
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ValidData_ShouldCreateInstance()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                id = 1,
                productname = \"Test Product\",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            // Assert
            orderRequest.id.Should().Be(1);
            orderRequest.productname.Should().Be(\"Test Product\");
 ...