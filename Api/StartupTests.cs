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

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Configure_HandlesDifferentEnvironments(bool isDevelopment, bool expectDeveloperExceptionPage)
        {
            // Arrange
            var services = new ServiceCollection();
            var applicationBuilder = new Mock<IApplicationBuilder>().Object;
            
            _mockEnvironment.Setup(e => e.EnvironmentName).Returns(isDevelopment ? "Development" : "Production");
            _mockEnvironment.Setup(e => e.IsDevelopment()).Returns(isDevelopment);

            var startup = new Startup(_mockConfiguration.Object);

            // Act & Assert
            var action = () => startup.Configure(applicationBuilder, _mockEnvironment.Object);
            action.Should().NotThrow();
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterProcessOrdersService()
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
    }
}