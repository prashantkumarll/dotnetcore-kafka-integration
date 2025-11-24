using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Confluent.Kafka;
using Api.Services;

namespace Api.Tests
{
    public class StartupTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;

        public StartupTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
        }

        [Fact]
        public void Constructor_ShouldInitializeConfiguration()
        {
            // Arrange & Act
            var startup = new Startup(_mockConfiguration.Object);

            // Assert
            startup.Configuration.Should().Be(_mockConfiguration.Object);
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterControllers()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            // Setup mock configuration sections
            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(Mock.Of<IConfigurationSection>());
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(Mock.Of<IConfigurationSection>());

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(ProcessOrdersService));
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterKafkaConfigs()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            // Setup mock configuration sections
            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(Mock.Of<IConfigurationSection>());
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(Mock.Of<IConfigurationSection>());

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(ProducerConfig));
            services.Should().Contain(s => s.ServiceType == typeof(ConsumerConfig));
        }

        [Fact]
        public void Configure_ShouldConfigureDevelopmentEnvironment()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns("Development");

            var startup = new Startup(_mockConfiguration.Object);

            // Act & Assert
            Action configureAction = () => startup.Configure(mockAppBuilder.Object, mockEnv.Object);
            configureAction.Should().NotThrow();
        }

        [Fact]
        public void Configure_ShouldConfigureProductionEnvironment()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns("Production");

            var startup = new Startup(_mockConfiguration.Object);

            // Act & Assert
            Action configureAction = () => startup.Configure(mockAppBuilder.Object, mockEnv.Object);
            configureAction.Should().NotThrow();
        }
    }
}