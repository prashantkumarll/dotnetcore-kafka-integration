using System;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Api.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Program_HostCreation_ShouldNotThrowException()
        {
            // Arrange
            string[] args = Array.Empty<string>();

            // Act
            Action hostCreation = () => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();

            // Assert
            hostCreation.Should().NotThrow();
        }
    }
}