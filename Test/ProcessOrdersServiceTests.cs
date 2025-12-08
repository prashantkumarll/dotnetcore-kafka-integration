using Api.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly ProcessOrdersService _service;
        private readonly Mock<ProcessOrdersService> _mockService;

        public ProcessOrdersServiceTests()
        {
            _service = new ProcessOrdersService();
            _mockService = new Mock<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_CanBeInstantiated()
        {
            // Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_IsOfCorrectType()
        {
            // Assert
            _service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_CanBeMocked()
        {
            // Arrange & Act
            var mockService = new Mock<ProcessOrdersService>();

            // Assert
            mockService.Should().NotBeNull();
            mockService.Object.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_MockObject_IsOfCorrectType()
        {
            // Assert
            _mockService.Object.Should().BeAssignableTo<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_AreDistinct()
        {
            // Arrange & Act
            var service1 = new ProcessOrdersService();
            var service2 = new ProcessOrdersService();

            // Assert
            service1.Should().NotBeSameAs(service2);
        }

        [Fact]
        public void ProcessOrdersService_DefaultConstructor_CreatesValidInstance()
        {
            // Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().NotBeNull();
            service.GetType().Should().Be<ProcessOrdersService>();
        }
    }
}