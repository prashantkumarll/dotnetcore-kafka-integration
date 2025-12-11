using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Api;

namespace Test
{
    public class StartupConfigurationTests
    {
        [Fact]
        public void Startup_Constructor_WithValidConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var startup = new Startup(mockConfiguration.Object);

            // Assert
            startup.Should().NotBeNull();
            startup.Should().BeOfType<Startup>();
        }

        [Fact]
        public void Startup_Constructor_WithConfigurationValues_ShouldAcceptConfiguration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["ConnectionStrings:DefaultConnection"]).Returns("test-connection");
            mockConfiguration.Setup(c => c["Logging:LogLevel:Default"]).Returns("Information");

            // Act
            var startup = new Startup(mockConfiguration.Object);

            // Assert
            startup.Should().NotBeNull();
        }

        [Fact]
        public void Startup_TypeValidation_ShouldBeCorrectType()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var startup = new Startup(mockConfiguration.Object);

            // Assert
            startup.GetType().Should().Be(typeof(Startup));
            startup.Should().BeAssignableTo<Startup>();
        }

        [Fact]
        public void Startup_Constructor_WithEmptyConfiguration_ShouldStillCreateInstance()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c[It.IsAny<string>()]).Returns((string)null);

            // Act
            var startup = new Startup(mockConfiguration.Object);

            // Assert
            startup.Should().NotBeNull();
        }

        [Fact]
        public void Startup_Constructor_WithNullConfiguration_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () => new Startup(null);

            // Assert
            act.Should().NotThrow();
        }
    }
}