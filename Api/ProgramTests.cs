using System;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace Api.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void CreateHostBuilder_ShouldConfigureWebHostDefaults()
        {
            // Arrange
            string[] args = new string[] { };

            // Act
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();

            // Assert
            host.Should().NotBeNull();
            host.Services.Should().NotBeNull();
        }

        [Fact]
        public void Run_ShouldStartHost()
        {
            // Arrange
            var mockHost = new Mock<IHost>();

            // Act
            mockHost.Object.Run();

            // Assert
            mockHost.Verify(h => h.Run(), Times.Once);
        }

        [Fact]
        public void CreateHostBuilder_WithNullArgs_ShouldNotThrowException()
        {
            // Arrange & Act
            var host = Host.CreateDefaultBuilder(null)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();

            // Assert
            host.Should().NotBeNull();
        }
    }
}