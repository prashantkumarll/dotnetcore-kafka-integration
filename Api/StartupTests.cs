using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Confluent.Kafka;
using Api.Services;

namespace Api.Tests
{
    public class StartupTests
    {
        [Fact]
        public void Constructor_ShouldInitializeConfiguration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>().Object;

            // Act
            var startup = new Startup(mockConfiguration);

            // Assert
            startup.Configuration.Should().NotBeNull();
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterControllers()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

            var startup = new Startup(mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(x => x.ServiceType == typeof(ProcessOrdersService));
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterKafkaConfigs()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

            var startup = new Startup(mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(x => x.ServiceType == typeof(ProducerConfig));
            services.Should().Contain(x => x.ServiceType == typeof(ConsumerConfig));
        }

        [Fact]
        public void Configure_ShouldConfigureDevelopmentEnvironment()
        {
            // Arrange
            var mockApp = new Mock<IApplicationBuilder>().Object;
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(x => x.EnvironmentName).Returns("Development");

            var startup = new Startup(new Mock<IConfiguration>().Object);

            // Act & Assert
            Action configureAction = () => startup.Configure(mockApp, mockEnv.Object);
            configureAction.Should().NotThrow();
        }

        [Fact]
        public void Configure_ShouldConfigureProductionEnvironment()
        {
            // Arrange
            var mockApp = new Mock<IApplicationBuilder>().Object;
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(x => x.EnvironmentName).Returns("Production");

            var startup = new Startup(new Mock<IConfiguration>().Object);

            // Act & Assert
            Action configureAction = () => startup.Configure(mockApp, mockEnv.Object);
            configureAction.Should().NotThrow();
        }
    }
}