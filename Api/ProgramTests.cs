using System;
using Xunit;
using Moq;
using FluentAssertions;
using Api;

namespace Test
{
    public class ProgramTests
    {
        [Fact]
        public void Program_TopLevelEntryPoint_Exists()
        {
            // Arrange & Act
            Action programExecution = () => {
                var args = new string[] {};
                var hostBuilder = Host.CreateDefaultBuilder(args);
                hostBuilder.ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
            };

            // Assert
            programExecution.Should().NotThrow();
        }
    }
}