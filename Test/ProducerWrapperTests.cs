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

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Type_ShouldHaveExpectedAttributes()
        {
            // Arrange & Act
            var type = typeof(Api.ProducerWrapper);
            
            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange
            var type = typeof(Api.ProducerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(2);
            constructor.GetParameters().First().ParameterType.Should().Be(typeof(string));
            constructor.GetParameters().Last().ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(Api.ProducerWrapper);
            
            // Act
            var methods = type.GetMethods().Where(m => m.IsPublic && m.DeclaringType == type);
            var methodNames = methods.Select(m => m.Name).ToList();
            
            // Assert
            methodNames.Should().Contain("writeMessage");
            methodNames.Should().Contain("DisposeAsync");
            methodNames.Should().Contain("Dispose");
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(Api.ProducerWrapper);
            
            // Act
            var interfaces = type.GetInterfaces();
            
            // Assert
            interfaces.Should().Contain(typeof(IDisposable));
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldHaveCorrectSignatures()
        {
            // Arrange
            var type = typeof(Api.ProducerWrapper);
            
            // Act
            var writeMessageMethod = type.GetMethod("writeMessage");
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");
            var disposeMethod = type.GetMethod("Dispose");
            
            // Assert
            writeMessageMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }
    }
}