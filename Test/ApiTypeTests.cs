using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Api;
using Api.Models;
using Api.Controllers;
using Api.Services;

namespace Test
{
    public class ApiTypeTests
    {
        [Fact]
        public void Api_Assembly_ShouldContainExpectedTypes()
        {
            // Arrange
            var apiAssembly = typeof(ProducerWrapper).Assembly;

            // Act
            var types = apiAssembly.GetTypes().Where(t => t.IsPublic);

            // Assert
            types.Should().NotBeEmpty();
            types.Should().Contain(t => t.Name == "ProducerWrapper");
            types.Should().Contain(t => t.Name == "ConsumerWrapper");
            types.Should().Contain(t => t.Name == "Startup");
            types.Should().Contain(t => t.Name == "OrderRequest");
            types.Should().Contain(t => t.Name == "OrderController");
            types.Should().Contain(t => t.Name == "ProcessOrdersService");
        }

        [Theory]
        [InlineData(typeof(ProducerWrapper), "Api")]
        [InlineData(typeof(ConsumerWrapper), "Api")]
        [InlineData(typeof(Startup), "Api")]
        [InlineData(typeof(OrderRequest), "Api.Models")]
        [InlineData(typeof(OrderController), "Api.Controllers")]
        [InlineData(typeof(ProcessOrdersService), "Api.Services")]
        public void Types_ShouldHaveCorrectNamespace(Type type, string expectedNamespace)
        {
            // Act
            var actualNamespace = type.Namespace;

            // Assert
            actualNamespace.Should().Be(expectedNamespace);
        }

        [Fact]
        public void OrderRequest_ShouldBeInModelsNamespace()
        {
            // Arrange
            var orderRequestType = typeof(OrderRequest);

            // Act
            var namespaceName = orderRequestType.Namespace;

            // Assert
            namespaceName.Should().Be("Api.Models");
        }

        [Fact]
        public void OrderController_ShouldBeInControllersNamespace()
        {
            // Arrange
            var orderControllerType = typeof(OrderController);

            // Act
            var namespaceName = orderControllerType.Namespace;

            // Assert
            namespaceName.Should().Be("Api.Controllers");
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInServicesNamespace()
        {
            // Arrange
            var processOrdersServiceType = typeof(ProcessOrdersService);

            // Act
            var namespaceName = processOrdersServiceType.Namespace;

            // Assert
            namespaceName.Should().Be("Api.Services");
        }

        [Fact]
        public void AllWrapperClasses_ShouldImplementIDisposable()
        {
            // Arrange
            var wrapperTypes = new[] { typeof(ProducerWrapper), typeof(ConsumerWrapper) };

            // Act & Assert
            foreach (var wrapperType in wrapperTypes)
            {
                wrapperType.Should().BeAssignableTo<IDisposable>();
                wrapperType.Should().BeAssignableTo<IAsyncDisposable>();
            }
        }

        [Fact]
        public void AllWrapperClasses_ShouldHaveStringConstructorParameters()
        {
            // Arrange
            var wrapperTypes = new[] { typeof(ProducerWrapper), typeof(ConsumerWrapper) };

            // Act & Assert
            foreach (var wrapperType in wrapperTypes)
            {
                var constructors = wrapperType.GetConstructors();
                var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 2);

                constructor.Should().NotBeNull();
                var parameters = constructor.GetParameters();
                parameters.Should().HaveCount(2);
                parameters.Should().OnlyContain(p => p.ParameterType == typeof(string));
            }
        }

        [Fact]
        public void ProcessOrdersService_Constructor_ShouldAcceptIConfiguration()
        {
            // Arrange
            var serviceType = typeof(ProcessOrdersService);

            // Act
            var constructors = serviceType.GetConstructors();
            var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);

            // Assert
            constructor.Should().NotBeNull();
            var parameter = constructor.GetParameters().First();
            parameter.ParameterType.Should().Be<IConfiguration>();
            parameter.Name.Should().Be("configuration");
        }
    }
}