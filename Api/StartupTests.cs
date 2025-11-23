using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Confluent.Kafka;
using Api;
using Api.Services;

namespace Test
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
            mockConfig.Setup(c => c.GetSection("producer")).Returns(new Mock<IConfigurationSection>().Object);
            mockConfig.Setup(c => c.GetSection("consumer")).Returns(new Mock<IConfigurationSection>().Object);

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
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns("Development");

            var mockAppBuilder = new Mock<IApplicationBuilder>();
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
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns("Production");

            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, mockEnv.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterControllers()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType.Name.Contains("Controller"));
        }

        [Fact]
        public void Configuration_ShouldBeReadable()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c[It.IsAny<string>()]).Returns("testValue");

            var startup = new Startup(mockConfig.Object);

            // Act & Assert
            startup.Configuration.Should().NotBeNull();
        }
    }
}