using Xunit;
using FluentAssertions;
using Api.Services;
using System;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_ShouldBeInstantiable()
        {
            // Arrange & Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().NotBeNull();
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
        public void ProcessOrdersService_ShouldBeReferenceType()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsClass.Should().BeTrue();
            type.IsValueType.Should().BeFalse();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHavePublicConstructor()
        {
            // Arrange & Act
            var constructors = typeof(ProcessOrdersService).GetConstructors();

            // Assert
            constructors.Should().NotBeEmpty();
            constructors.Should().Contain(c => c.IsPublic);
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeDifferentObjects()
        {
            // Arrange & Act
            var service1 = new ProcessOrdersService();
            var service2 = new ProcessOrdersService();

            // Assert
            service1.Should().NotBeSameAs(service2);
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_TypeShouldBePublic()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsPublic.Should().BeTrue();
        }
    }
}