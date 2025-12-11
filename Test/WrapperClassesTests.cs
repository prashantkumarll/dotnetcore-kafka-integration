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
    public class WrapperClassesTests
    {
        [Fact]
        public void ProducerWrapper_Type_ShouldHaveExpectedStructure()
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);

            // Act
            var constructors = producerType.GetConstructors();
            var methods = producerType.GetMethods();

            // Assert
            producerType.Should().NotBeNull();
            producerType.Namespace.Should().Be("Api");
            constructors.Should().NotBeEmpty();
            
            var stringConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters().All(p => p.ParameterType == typeof(string)));
            
            stringConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldExist()
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);

            // Act
            var writeMessageMethod = producerType.GetMethods()
                .FirstOrDefault(m => m.Name == "writeMessage");
            var disposeAsyncMethod = producerType.GetMethods()
                .FirstOrDefault(m => m.Name == "DisposeAsync");
            var disposeMethod = producerType.GetMethods()
                .FirstOrDefault(m => m.Name == "Dispose");

            // Assert
            writeMessageMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedStructure()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var constructors = consumerType.GetConstructors();
            var methods = consumerType.GetMethods();

            // Assert
            consumerType.Should().NotBeNull();
            consumerType.Namespace.Should().Be("Api");
            constructors.Should().NotBeEmpty();
            
            var stringConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 2 && 
                c.GetParameters().All(p => p.ParameterType == typeof(string)));
            
            stringConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Methods_ShouldExist()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var readMessageAsyncMethod = consumerType.GetMethods()
                .FirstOrDefault(m => m.Name == "ReadMessageAsync");
            var disposeAsyncMethod = consumerType.GetMethods()
                .FirstOrDefault(m => m.Name == "DisposeAsync");
            var disposeMethod = consumerType.GetMethods()
                .FirstOrDefault(m => m.Name == "Dispose");

            // Assert
            readMessageAsyncMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Theory]
        [InlineData("connection-string-1", "topic-1")]
        [InlineData("connection-string-2", "topic-2")]
        [InlineData("", "")]
        public void WrapperClasses_ConstructorParameters_ShouldMatchPattern(string connectionString, string topicName)
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var producerConstructor = producerType.GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length == 2);
            var consumerConstructor = consumerType.GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length == 2);

            // Assert
            producerConstructor.Should().NotBeNull();
            consumerConstructor.Should().NotBeNull();
            
            var producerParams = producerConstructor.GetParameters();
            var consumerParams = consumerConstructor.GetParameters();
            
            producerParams.Should().HaveCount(2);
            consumerParams.Should().HaveCount(2);
            
            producerParams[0].ParameterType.Should().Be(typeof(string));
            producerParams[1].ParameterType.Should().Be(typeof(string));
            consumerParams[0].ParameterType.Should().Be(typeof(string));
            consumerParams[1].ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void WrapperClasses_ShouldImplementDisposalPattern()
        {
            // Arrange
            var producerType = typeof(ProducerWrapper);
            var consumerType = typeof(ConsumerWrapper);

            // Act
            var producerInterfaces = producerType.GetInterfaces();
            var consumerInterfaces = consumerType.GetInterfaces();
            
            var producerMethods = producerType.GetMethods().Select(m => m.Name).ToList();
            var consumerMethods = consumerType.GetMethods().Select(m => m.Name).ToList();

            // Assert
            producerMethods.Should().Contain("Dispose");
            producerMethods.Should().Contain("DisposeAsync");
            consumerMethods.Should().Contain("Dispose");
            consumerMethods.Should().Contain("DisposeAsync");
        }
    }
}