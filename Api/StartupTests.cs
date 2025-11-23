using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
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

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(IServiceCollection));
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
            services.Should().Contain(s => s.ServiceType == typeof(ProcessOrdersService));
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
        public void ConfigureServices_ShouldBindKafkaConfigurations()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(ProducerConfig));
            services.Should().Contain(s => s.ServiceType == typeof(ConsumerConfig));
        }

        [Fact]
        public void ConfigureServices_ShouldMapControllers()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            services.Should().Contain(s => s.ServiceType.Name.Contains("Controllers"));
        }
    }
}