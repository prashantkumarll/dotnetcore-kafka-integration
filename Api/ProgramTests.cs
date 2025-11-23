using System;
using Xunit;
using Moq;
using FluentAssertions;
using Api;
using Microsoft.Extensions.Logging;

namespace Test
{
    public class ProgramTests
    {
        [Fact]
        public void Program_Startup_Initialization_Successful()
        {
            // Arrange & Act
            Action programInitialization = () => {
                var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                    .Build();

                host.Run();
            };

            // Assert
            programInitialization.Should().NotThrow();
        }
    }
}