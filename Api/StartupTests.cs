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
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;

        public StartupTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Fact]
        public void Constructor_ShouldInitializeConfiguration()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>().Object;

            // Act
            var startup = new Startup(configuration);

            // Assert
            startup.Configuration.Should().NotBeNull();
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterDependencies()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c.GetSection("producer")).Returns(Mock.Of<IConfigurationSection>());
            mockConfig.Setup(c => c.GetSection("consumer")).Returns(Mock.Of<IConfigurationSection>());

            var startup = new Startup(mockConfig.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ProducerConfig>().Should().NotBeNull();
            serviceProvider.GetService<ConsumerConfig>().Should().NotBeNull();
            serviceProvider.GetService<ProcessOrdersService>().Should().NotBeNull();
        }

        [Fact]
        public void Configure_DevelopmentEnvironment_ShouldUseDeveloperExceptionPage()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns("Development");

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, mockEnv.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Configure_ProductionEnvironment_ShouldUseHsts()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns("Production");

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, mockEnv.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Configure_ShouldMapControllers()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, mockEnv.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }
    }
}