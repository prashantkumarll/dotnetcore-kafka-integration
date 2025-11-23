using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Test
{
    public class ProgramTests
    {
        [Fact]
        public void Program_Startup_ShouldCreateHostBuilder()
        {
            // Arrange & Act
            Action hostCreation = () => Host.CreateDefaultBuilder(null)
                .ConfigureWebHostDefaults(webBuilder => 
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();

            // Assert
            hostCreation.Should().NotThrow();
        }

        [Fact]
        public void Startup_ConfigureServices_ShouldNotThrowException()
        {
            // Arrange
            var startup = new Startup(null);

            // Act
            Action configureServices = () => startup.ConfigureServices(null);

            // Assert
            configureServices.Should().NotThrow();
        }

        [Fact]
        public void Startup_Configure_ShouldNotThrowException()
        {
            // Arrange
            var startup = new Startup(null);

            // Act
            Action configure = () => startup.Configure(null, null);

            // Assert
            configure.Should().NotThrow();
        }
    }
}