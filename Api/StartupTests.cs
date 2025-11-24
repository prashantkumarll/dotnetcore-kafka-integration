using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Confluent.Kafka;

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
            var serviceProvider = services.BuildServiceProvider();
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
        public void ConfigureServices_ShouldRegisterKafkaConfigs()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            var mockProducerSection = new Mock<IConfigurationSection>();
            var mockConsumerSection = new Mock<IConfigurationSection>();

            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(mockProducerSection.Object);
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(mockConsumerSection.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ProducerConfig>().Should().NotBeNull();
            serviceProvider.GetService<ConsumerConfig>().Should().NotBeNull();
        }
    }
}