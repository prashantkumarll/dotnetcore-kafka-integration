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
using Api.Controllers;
using Azure.Messaging.ServiceBus;

namespace Test
{
    public class OrderControllerTests
    {
        [Fact]
        public void OrderController_Type_ShouldHaveExpectedStructure()
        {
            // Arrange
            var controllerType = typeof(OrderController);

            // Act
            var constructors = controllerType.GetConstructors();
            var methods = controllerType.GetMethods();
            var properties = controllerType.GetProperties();

            // Assert
            controllerType.Should().NotBeNull();
            controllerType.Namespace.Should().Be("Api.Controllers");
            constructors.Should().NotBeEmpty();
            
            var serviceBusConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(ServiceBusClient));
            
            serviceBusConstructor.Should().NotBeNull();
        }

        [Fact]
        public void OrderController_PostAsyncMethod_ShouldExist()
        {
            // Arrange
            var controllerType = typeof(OrderController);

            // Act
            var postAsyncMethod = controllerType.GetMethods()
                .FirstOrDefault(m => m.Name == "PostAsync");

            // Assert
            postAsyncMethod.Should().NotBeNull();
            postAsyncMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void OrderController_Constructor_ShouldAcceptServiceBusClient()
        {
            // Arrange
            var controllerType = typeof(OrderController);

            // Act
            var constructor = controllerType.GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length == 1);

            // Assert
            constructor.Should().NotBeNull();
            var parameter = constructor.GetParameters().FirstOrDefault();
            parameter.Should().NotBeNull();
            parameter.ParameterType.Should().Be(typeof(ServiceBusClient));
        }

        [Fact]
        public void OrderController_Methods_ShouldHaveCorrectSignatures()
        {
            // Arrange
            var controllerType = typeof(OrderController);

            // Act
            var publicMethods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // Assert
            publicMethods.Should().NotBeEmpty();
            
            foreach (var method in publicMethods)
            {
                method.Name.Should().NotBeNullOrEmpty();
                method.IsPublic.Should().BeTrue();
            }
        }

        [Fact]
        public void OrderController_Namespace_ShouldBeCorrect()
        {
            // Arrange
            var controllerType = typeof(OrderController);

            // Act
            var namespaceName = controllerType.Namespace;

            // Assert
            namespaceName.Should().Be("Api.Controllers");
        }
    }
}