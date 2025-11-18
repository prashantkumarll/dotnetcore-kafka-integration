using Xunit;
using NUnit.Framework;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Api.Tests
{
    [TestClass]
    public class StartupHostTests
    {
        [Fact]
        [Test]
        public void HostBuilder_ConfiguresWebHost_Successfully()
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
            Assert.NotNull(host);
        }

        [Fact]
        [Test]
        public void HostBuilder_WithNullArgs_HandlesGracefully()
        {
            // Arrange & Act
            var host = Host.CreateDefaultBuilder(null)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();

            // Assert
            Assert.NotNull(host);
        }

        [Fact]
        [Test]
        public void HostBuilder_ConfiguresStartup_Correctly()
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
            Assert.NotNull(host.Services.GetService(typeof(Startup)));
        }
    }
}