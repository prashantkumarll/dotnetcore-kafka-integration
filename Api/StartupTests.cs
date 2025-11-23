using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Confluent.Kafka;
using Api;
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
        public void Configure_ShouldConfigureDevelopmentEnvironment()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns(Environments.Development);

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, mockEnv.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Configure_ShouldConfigureProductionEnvironment()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns(Environments.Production);

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, mockEnv.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterKafkaConfigs()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            var mockProducerConfig = new ProducerConfig();
            var mockConsumerConfig = new ConsumerConfig();

            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(Mock.Of<IConfigurationSection>());
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(Mock.Of<IConfigurationSection>());

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(ProducerConfig));
            services.Should().Contain(s => s.ServiceType == typeof(ConsumerConfig));
        }
    }
}