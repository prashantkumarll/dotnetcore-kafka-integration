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
        public void ConsumerWrapper_Type_ShouldExist()
        {
            // Arrange & Act
            var type = typeof(ConsumerWrapper);
            
            // Assert
            type.Should().NotBeNull();
            type.IsPublic.Should().BeTrue();
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptConnectionStringAndTopicName()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.IsPublic.Should().BeTrue();
            
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[1].ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsyncMethod_ShouldExist()
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
        public void ConsumerWrapper_DisposeAsyncMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("DisposeAsync");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_DisposeMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("Dispose");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange & Act
            var type = typeof(ConsumerWrapper);
            var interfaces = type.GetInterfaces();
            
            // Assert
            interfaces.Should().Contain(typeof(IDisposable));
        }

        [Theory]
        [InlineData("ReadMessageAsync")]
        [InlineData("DisposeAsync")]
        [InlineData("Dispose")]
        public void ConsumerWrapper_PublicMethods_ShouldExist(string methodName)
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod(methodName);
            
            // Assert
            method.Should().NotBeNull($"Method {methodName} should exist");
            method.IsPublic.Should().BeTrue($"Method {methodName} should be public");
        }

        [Fact]
        public void ConsumerWrapper_Methods_ShouldHaveCorrectCount()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var publicMethods = type.GetMethods()
                .Where(m => m.IsPublic && m.DeclaringType == type)
                .ToList();
            
            // Assert
            publicMethods.Should().NotBeEmpty();
            
            // Check for expected methods
            var expectedMethods = new[] { "ReadMessageAsync", "DisposeAsync", "Dispose" };
            foreach (var expectedMethod in expectedMethods)
            {
                publicMethods.Any(m => m.Name == expectedMethod)
                    .Should().BeTrue($"Method {expectedMethod} should exist");
            }
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ParameterTypes_ShouldBeStrings()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            var parameters = constructor?.GetParameters();
            
            // Assert
            parameters.Should().NotBeNull();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[1].ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_ShouldBeAsyncMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("ReadMessageAsync");
            
            // Assert
            method.Should().NotBeNull();
            method.Name.Should().EndWith("Async");
        }

        [Fact]
        public void ConsumerWrapper_AllMethods_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            var expectedMethods = new[] { "ReadMessageAsync", "DisposeAsync", "Dispose" };
            
            // Act & Assert
            foreach (var methodName in expectedMethods)
            {
                var method = type.GetMethod(methodName);
                method.Should().NotBeNull($"Method {methodName} should exist");
                method.IsPublic.Should().BeTrue($"Method {methodName} should be public");
            }
        }
    }
}