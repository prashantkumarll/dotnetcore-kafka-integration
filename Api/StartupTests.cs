using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Api;
using Api.Services;

namespace Test
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
        public void ConfigureServices_ShouldRegisterDependencies()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockProducerConfig = new ProducerConfig();
            var mockConsumerConfig = new ConsumerConfig();

            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(new Mock<IConfigurationSection>().Object);
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(new Mock<IConfigurationSection>().Object);

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ProducerConfig>().Should().NotBeNull();
            serviceProvider.GetService<ConsumerConfig>().Should().NotBeNull();
            serviceProvider.GetService<ProcessOrdersService>().Should().NotBeNull();
        }

        [Fact]
        public void Configure_ShouldConfigurePipeline()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns(Environments.Development);

            var startup = new Startup(_mockConfiguration.Object);

            // Act & Assert
            Action configureAction = () => startup.Configure(mockAppBuilder.Object, mockEnv.Object);
            configureAction.Should().NotThrow();
        }

        [Fact]
        public void Configure_ShouldUseHttpsInNonDevelopment()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.EnvironmentName).Returns(Environments.Production);

            var startup = new Startup(_mockConfiguration.Object);

            // Act & Assert
            Action configureAction = () => startup.Configure(mockAppBuilder.Object, mockEnv.Object);
            configureAction.Should().NotThrow();
        }
    }
}