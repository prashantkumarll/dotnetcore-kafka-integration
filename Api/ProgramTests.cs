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
        public void Program_Startup_ShouldCreateHostSuccessfully()
        {
            // Arrange & Act
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
            programExecution.Should().NotThrow("Program startup should complete without exceptions");
        }
    }
}