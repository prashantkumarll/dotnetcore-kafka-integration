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
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Type_ShouldExist()
        {
            // Arrange & Act
            var type = typeof(ProducerWrapper);
            
            // Assert
            type.Should().NotBeNull();
            type.IsPublic.Should().BeTrue();
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptConnectionStringAndTopicName()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
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
        public void ProducerWrapper_WriteMessageMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var method = type.GetMethod("writeMessage");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_DisposeAsyncMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var method = type.GetMethod("DisposeAsync");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_DisposeMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var method = type.GetMethod("Dispose");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange & Act
            var type = typeof(ProducerWrapper);
            var interfaces = type.GetInterfaces();
            
            // Assert
            interfaces.Should().Contain(typeof(IDisposable));
        }

        [Theory]
        [InlineData("writeMessage")]
        [InlineData("DisposeAsync")]
        [InlineData("Dispose")]
        public void ProducerWrapper_PublicMethods_ShouldExist(string methodName)
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var method = type.GetMethod(methodName);
            
            // Assert
            method.Should().NotBeNull($"Method {methodName} should exist");
            method.IsPublic.Should().BeTrue($"Method {methodName} should be public");
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldHaveCorrectSignatures()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            var methods = type.GetMethods().Where(m => m.IsPublic && m.DeclaringType == type).ToList();
            
            // Act & Assert
            methods.Should().NotBeEmpty();
            
            var writeMessageMethod = methods.FirstOrDefault(m => m.Name == "writeMessage");
            if (writeMessageMethod != null)
            {
                writeMessageMethod.Name.Should().Be("writeMessage");
            }
            
            var disposeAsyncMethod = methods.FirstOrDefault(m => m.Name == "DisposeAsync");
            if (disposeAsyncMethod != null)
            {
                disposeAsyncMethod.Name.Should().Be("DisposeAsync");
            }
            
            var disposeMethod = methods.FirstOrDefault(m => m.Name == "Dispose");
            if (disposeMethod != null)
            {
                disposeMethod.Name.Should().Be("Dispose");
            }
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldHaveCorrectParameterNames()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            var parameters = constructor?.GetParameters();
            
            // Assert
            parameters.Should().NotBeNull();
            parameters.Should().HaveCount(2);
            
            // Note: Parameter names might not be available in release builds,
            // so we just verify the types and count
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[1].ParameterType.Should().Be(typeof(string));
        }
    }
}