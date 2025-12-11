using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Api;

namespace Test
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Type_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptCorrectParameters()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var constructors = type.GetConstructors();
            var twoParamConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters()[0].ParameterType == typeof(string) &&
                c.GetParameters()[1].ParameterType == typeof(string));

            // Assert
            constructors.Should().NotBeNull();
            twoParamConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldExist()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var methods = type.GetMethods();
            var writeMessageMethod = methods.FirstOrDefault(m => m.Name == "writeMessage");
            var disposeAsyncMethod = methods.FirstOrDefault(m => m.Name == "DisposeAsync");
            var disposeMethod = methods.FirstOrDefault(m => m.Name == "Dispose");

            // Assert
            writeMessageMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            isPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldNotBeAbstract()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var isAbstract = type.IsAbstract;

            // Assert
            isAbstract.Should().BeFalse();
        }

        [Fact]
        public void ProducerWrapper_Constructor_Parameters_ShouldHaveCorrectNames()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 2);
            var parameters = constructor?.GetParameters();

            // Assert
            parameters.Should().NotBeNull();
            parameters.Should().HaveCount(2);
            parameters[0].ParameterType.Should().Be<string>();
            parameters[1].ParameterType.Should().Be<string>();
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var writeMessage = type.GetMethod("writeMessage");
            var disposeAsync = type.GetMethod("DisposeAsync");
            var dispose = type.GetMethod("Dispose");

            // Assert
            writeMessage?.IsPublic.Should().BeTrue();
            disposeAsync?.IsPublic.Should().BeTrue();
            dispose?.IsPublic.Should().BeTrue();
        }
    }
}