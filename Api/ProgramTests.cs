using System;
using Xunit;
using Moq;
using FluentAssertions;
using Api;
using Microsoft.Extensions.Hosting;

namespace Test
{
    public class ProgramTests
    {
        [Fact]
        public void Program_HostCreation_ShouldCreateValidHost()
        {
            // Arrange & Act
            var host = Host.CreateDefaultBuilder(Array.Empty<string>())
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