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

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedAttributes()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedConstructor()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters()[0].ParameterType == typeof(string) &&
                c.GetParameters()[1].ParameterType == typeof(string));

            // Assert
            targetConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveReadMessageAsyncMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var method = type.GetMethod("ReadMessageAsync");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var implementsIDisposable = typeof(IDisposable).IsAssignableFrom(type);

            // Assert
            implementsIDisposable.Should().BeTrue();
        }
    }
}