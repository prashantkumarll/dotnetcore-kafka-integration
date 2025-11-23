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
        public void Program_Startup_ShouldCreateHostSuccessfully()
        {
            // Arrange & Act
            Action startupAction = () => {
                var host = Host.CreateDefaultBuilder(null)
                    .ConfigureWebHostDefaults(webBuilder => {
                        webBuilder.UseStartup<Startup>();
                    })
                    .Build();

                host.Run();
            };

            // Assert
            startupAction.Should().NotThrow();
        }

        [Fact]
        public void Startup_ConfigureServices_ShouldRegisterDependencies()
        {
            // Arrange
            var startup = new Startup();
            var services = new Mock<IServiceCollection>().Object;

            // Act & Assert
            Action configureServicesAction = () => {
                startup.ConfigureServices(services);
            };

            configureServicesAction.Should().NotThrow();
        }

        [Fact]
        public void Startup_Configure_ShouldSetupMiddleware()
        {
            // Arrange
            var startup = new Startup();
            var app = new Mock<IApplicationBuilder>().Object;
            var env = new Mock<IWebHostEnvironment>().Object;

            // Act & Assert
            Action configureAction = () => {
                startup.Configure(app, env);
            };

            configureAction.Should().NotThrow();
        }
    }
}