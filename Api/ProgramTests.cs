using System;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Hosting;

namespace Test
{
    public class ProgramTests
    {
        [Fact]
        public void Program_HostCreation_ShouldNotThrowException()
        {
            // Arrange
            Action hostCreation = () => 
            {
                var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                    .ConfigureWebHostDefaults(webBuilder => {})
                    .Build();
            };

            // Act & Assert
            hostCreation.Should().NotThrow();
        }
    }
}