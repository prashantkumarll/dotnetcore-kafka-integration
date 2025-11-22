{
  "testCasesFound": 0,
  "newTestCasesAdded": 3,
  "generatedTestCode": "using System;
using Xunit;
using NUnit.Framework;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace Api.Tests
{
    [TestClass]
    public class HostStartupTests
    {
        [Fact]
        public void HostBuilder_ConfiguresWebHost_Successfully()
        {
            // Arrange
            var args = new string[] { };

            // Act
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();

            // Assert
            Assert.NotNull(host);
        }

        [Fact]
        public void HostBuilder_WithNullArgs_HandlesGracefully()
        {
            // Act
            var host = Host.CreateDefaultBuilder(null)
                .ConfigureWebHostDefaults(webBuilder =>
                {
     ...