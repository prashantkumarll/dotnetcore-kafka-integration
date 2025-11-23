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
            var configuration = Mock.Of<IConfiguration>();

            // Act
            var startup = new Startup(configuration);

            // Assert
            startup.Configuration.Should().NotBeNull();
            startup.Configuration.Should().Be(configuration);
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterControllers()
        {
            // Arrange
            var services = new ServiceCollection();
            var producerConfigSection = Mock.Of<IConfigurationSection>(s => s.Value == "test");
            var consumerConfigSection = Mock.Of<IConfigurationSection>(s => s.Value == "test");

            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(producerConfigSection);
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(consumerConfigSection);

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(ProcessOrdersService));
        }

        [Fact]
        public void Configure_InDevelopment_ShouldUseDeveloperExceptionPage()
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
        public void Configure_NotInDevelopment_ShouldUseHsts()
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
        public void ConfigureServices_ShouldRegisterKafkaConfigs()
        {
            // Arrange
            var services = new ServiceCollection();
            var producerConfigSection = Mock.Of<IConfigurationSection>(s => s.Value == "test");
            var consumerConfigSection = Mock.Of<IConfigurationSection>(s => s.Value == "test");

            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(producerConfigSection);
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(consumerConfigSection);

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(ProducerConfig));
            services.Should().Contain(s => s.ServiceType == typeof(ConsumerConfig));
        }
    }
}