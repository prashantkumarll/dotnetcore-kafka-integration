using Api.Services;
using Xunit;
using FluentAssertions;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_ShouldBeInstantiable()
        {
            // Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveCorrectType()
        {
            // Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveCorrectNamespace()
        {
            // Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeReferenceType()
        {
            // Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsClass.Should().BeTrue();
            type.IsValueType.Should().BeFalse();
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeUnique()
        {
            // Act
            var service1 = new ProcessOrdersService();
            var service2 = new ProcessOrdersService();

            // Assert
            service1.Should().NotBeSameAs(service2);
        }
    }
}