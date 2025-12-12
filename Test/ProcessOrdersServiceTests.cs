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
using Api.Services;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveCorrectProperties()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProcessOrdersService");
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Constructor_ShouldRequireIConfiguration()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var constructors = type.GetConstructors();
            var mainConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);

            // Assert
            mainConstructor.Should().NotBeNull();
            var parameter = mainConstructor.GetParameters().First();
            parameter.ParameterType.Should().Be(typeof(IConfiguration));
        }
    }
}