using Api.Services;
using FluentAssertions;
using System;
using Xunit;

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
        public void ProcessOrdersService_ShouldBeReferenceType()
        {
            // Act & Assert
            typeof(ProcessOrdersService).Should().NotBeValueType();
            typeof(ProcessOrdersService).IsClass.Should().BeTrue();
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
        public void ProcessOrdersService_ShouldBeInCorrectNamespace()
        {
            // Act
            var service = new ProcessOrdersService();

            // Assert
            service.GetType().Namespace.Should().Be("Api.Services");
            service.GetType().FullName.Should().Be("Api.Services.ProcessOrdersService");
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeDifferentObjects()
        {
            // Act
            var service1 = new ProcessOrdersService();
            var service2 = new ProcessOrdersService();

            // Assert
            service1.Should().NotBeSameAs(service2);
            ReferenceEquals(service1, service2).Should().BeFalse();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHavePublicType()
        {
            // Act & Assert
            typeof(ProcessOrdersService).IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_ShouldNotBeAbstract()
        {
            // Act & Assert
            typeof(ProcessOrdersService).IsAbstract.Should().BeFalse();
        }
    }
}