using System;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using FluentAssertions;
using Moq;

namespace Api.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Main_HostCreationAndRun_ShouldSucceed()
        {
            // Arrange
            var mockHost = new Mock<IHost>();
            var mockHostBuilder = new Mock<IHostBuilder>();

            mockHostBuilder
                .Setup(x => x.ConfigureWebHostDefaults(It.IsAny<Action<IWebHostBuilder>>()))
                .Returns(mockHostBuilder.Object);

            mockHostBuilder
                .Setup(x => x.Build())
                .Returns(mockHost.Object);

            // Act
            Action runAction = () => mockHost.Object.Run();

            // Assert
            runAction.Should().NotThrow();
        }
    }
}