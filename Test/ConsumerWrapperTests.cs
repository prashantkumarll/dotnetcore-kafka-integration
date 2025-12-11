using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Api;

namespace Test
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptTwoStringParameters()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var twoStringParamConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters().All(p => p.ParameterType == typeof(string)));

            // Assert
            constructors.Should().NotBeNull();
            twoStringParamConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_RequiredMethods_ShouldExist()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var methods = type.GetMethods();
            var readMessageAsyncMethod = methods.FirstOrDefault(m => m.Name == "ReadMessageAsync");
            var disposeAsyncMethod = methods.FirstOrDefault(m => m.Name == "DisposeAsync");
            var disposeMethod = methods.FirstOrDefault(m => m.Name == "Dispose");

            // Assert
            readMessageAsyncMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            isPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldNotBeInterface()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var isInterface = type.IsInterface;

            // Assert
            isInterface.Should().BeFalse();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_Parameters_ShouldBeStringType()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 2);
            var parameters = constructor?.GetParameters();

            // Assert
            parameters.Should().NotBeNull();
            parameters.Should().HaveCount(2);
            parameters.Should().OnlyContain(p => p.ParameterType == typeof(string));
        }

        [Fact]
        public void ConsumerWrapper_Methods_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var readMessageAsync = type.GetMethod("ReadMessageAsync");
            var disposeAsync = type.GetMethod("DisposeAsync");
            var dispose = type.GetMethod("Dispose");

            // Assert
            readMessageAsync?.IsPublic.Should().BeTrue();
            disposeAsync?.IsPublic.Should().BeTrue();
            dispose?.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_AllMethods_ShouldNotBeStatic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var readMessageAsync = type.GetMethod("ReadMessageAsync");
            var disposeAsync = type.GetMethod("DisposeAsync");
            var dispose = type.GetMethod("Dispose");

            // Assert
            readMessageAsync?.IsStatic.Should().BeFalse();
            disposeAsync?.IsStatic.Should().BeFalse();
            dispose?.IsStatic.Should().BeFalse();
        }
    }
}