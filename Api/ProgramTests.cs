using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Api;

namespace Test
{
    public class ProgramTests
    {
        [Fact]
        public void TopLevelProgram_ShouldCreateHostAndRun()
        {
            // Arrange & Act - Verify top-level program can be instantiated without errors
            Action programExecution = () => {
                var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                    .Build();

                host.Run();
            };

            // Assert
            programExecution.Should().NotThrow("Program should initialize and run without exceptions");
        }
    }
}