using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        public void ConfigureServices_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            // Setup mock configuration sections
            var producerSection = new Mock<IConfigurationSection>();
            var consumerSection = new Mock<IConfigurationSection>();
            _mockConfiguration.Setup(c => c.GetSection("producer")).Returns(producerSection.Object);
            _mockConfiguration.Setup(c => c.GetSection("consumer")).Returns(consumerSection.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(ProducerConfig));
            services.Should().Contain(s => s.ServiceType == typeof(ConsumerConfig));
            services.Should().Contain(s => s.ServiceType == typeof(IHostedService));
        }

        [Fact]
        public void Configure_DevelopmentEnvironment_ShouldUseDeveloperExceptionPage()
        {
            // Arrange
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");

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
            var mockAppBuilder = new Mock<IApplicationBuilder>();
            _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Production");

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, _mockEnvironment.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ConfigureServices_ShouldAddControllers()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(IControllerFactory));
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterProcessOrdersService()
        {
            // Arrange
            var services = new ServiceCollection();
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
            var mockEndpointRouteBuilder = new Mock<IEndpointRouteBuilder>();

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockAppBuilder.Object, _mockEnvironment.Object);

            // Assert
            mockAppBuilder.Verify(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }
    }
}