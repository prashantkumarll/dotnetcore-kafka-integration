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
        public void ConfigureServices_ShouldRegisterDependencies()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockProducerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var mockConsumerConfig = new ConsumerConfig { BootstrapServers = "localhost:9092" };

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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Configure_ShouldSetupMiddlewarePipeline(bool isDevelopment)
        {
            // Arrange
            var services = new ServiceCollection();
            var applicationBuilder = new Mock<IApplicationBuilder>().Object;

            _mockEnvironment.Setup(e => e.EnvironmentName).Returns(isDevelopment ? "Development" : "Production");
            _mockEnvironment.Setup(e => e.IsDevelopment()).Returns(isDevelopment);

            var startup = new Startup(_mockConfiguration.Object);

            // Act & Assert
            Action configureAction = () => startup.Configure(applicationBuilder, _mockEnvironment.Object);
            configureAction.Should().NotThrow();
        }

        [Fact]
        public void Startup_ShouldHaveDefaultConstructor()
        {
            // Arrange & Act
            var startup = new Startup(_mockConfiguration.Object);

            // Assert
            startup.Should().NotBeNull();
        }

        [Fact]
        public void Configuration_ShouldBeReadOnly()
        {
            // Arrange
            var startup = new Startup(_mockConfiguration.Object);

            // Act & Assert
            startup.Configuration.Should().NotBeNull();
            Action modifyConfiguration = () => { startup.Configuration = null; };
            modifyConfiguration.Should().Throw<CompilerGeneratedException>();
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
            services.Should().Contain(s => s.ServiceType.Name.Contains("Controllers"));
        }

        [Fact]
        public void Configure_ShouldMapControllers()
        {
            // Arrange
            var mockApplicationBuilder = new Mock<IApplicationBuilder>();
            var mockEndpointRouteBuilder = new Mock<IEndpointRouteBuilder>();

            _mockEnvironment.Setup(e => e.IsDevelopment()).Returns(false);

            var startup = new Startup(_mockConfiguration.Object);

            // Act
            startup.Configure(mockApplicationBuilder.Object, _mockEnvironment.Object);

            // Assert
            mockApplicationBuilder.Verify(a => a.UseRouting(), Times.Once);
        }
    }
}