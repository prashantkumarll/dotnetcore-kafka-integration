using Api.Services;
using Xunit;
using FluentAssertions;
using System;

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
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var service = new ProcessOrdersService();

            // Act
            var namespaceName = service.GetType().Namespace;

            // Assert
            namespaceName.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveParameterlessConstructor()
        {
            // Act
            var constructor = typeof(ProcessOrdersService).GetConstructor(Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeDifferentObjects()
        {
            // Act
            var service1 = new ProcessOrdersService();
            var service2 = new ProcessOrdersService();

            // Assert
            service1.Should().NotBeSameAs(service2);
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_TypeShouldBeClass()
        {
            // Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsClass.Should().BeTrue();
            type.IsInterface.Should().BeFalse();
            type.IsAbstract.Should().BeFalse();
        }
    }
}