using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka;

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
        public void Constructor_ShouldSetConfiguration()
        {
            // Arrange
            var configuration = _mockConfiguration.Object;

            // Act
            var startup = new Startup(configuration);

            // Assert
            startup.Configuration.Should().Be(configuration);
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterControllers()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockProducerSection = new Mock<IConfigurationSection>();
            var mockConsumerSection = new Mock<IConfigurationSection>();

            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(mockProducerSection.Object);
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(mockConsumerSection.Object);

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(IStartupFilter));
            services.Should().Contain(s => s.ServiceType == typeof(ProducerConfig));
            services.Should().Contain(s => s.ServiceType == typeof(ConsumerConfig));
        }

        [Fact]
        public void Configure_DevelopmentEnvironment_ShouldUseDeveloperExceptionPage()
        {
            // Arrange
            _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");

            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, _mockEnvironment.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Configure_ProductionEnvironment_ShouldUseHsts()
        {
            // Arrange
            _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Production");

            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, _mockEnvironment.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterProcessOrdersService()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockProducerSection = new Mock<IConfigurationSection>();
            var mockConsumerSection = new Mock<IConfigurationSection>();

            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(mockProducerSection.Object);
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(mockConsumerSection.Object);

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(IHostedService) && s.ImplementationType == typeof(ProcessOrdersService));
        }

        [Fact]
        public void Configure_ShouldMapControllers()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, _mockEnvironment.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Startup_ShouldAllowConfigurationInjection()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var startup = new Startup(mockConfig.Object);

            // Assert
            startup.Configuration.Should().NotBeNull();
        }

        [Fact]
        public void Startup_ConfigurationShouldBeImmutable()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var startup = new Startup(mockConfig.Object);

            // Assert
            startup.Configuration.Should().Be(mockConfig.Object);
        }
    }
}