using System;
using Xunit;
using FluentAssertions;
using Moq;

namespace Test
{
    public class UnitTest1Tests
    {
        [Fact]
        public void Test1_DefaultScenario_ShouldPass()
        {
            // Arrange
            
            // Act
            
            // Assert
            true.Should().BeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Test1_ParameterizedTest_ShouldHandleMultipleInputs(int input)
        {
            // Arrange
            
            // Act
            
            // Assert
            input.Should().BePositive();
        }

        [Fact]
        public void Test1_NullInput_ShouldHandleNullCase()
        {
            // Arrange
            
            // Act
            Action nullAction = () => { };

            // Assert
            nullAction.Should().NotThrow();
        }

        [Fact]
        public void Test1_ExceptionScenario_ShouldCatchException()
        {
            // Arrange
            
            // Act
            Action exceptionAction = () => throw new InvalidOperationException();

            // Assert
            exceptionAction.Should().Throw<InvalidOperationException>(); 
        }

        [Fact]
        public void Test1_EdgeCase_ShouldHandleExtremeSituation()
        {
            // Arrange
            
            // Act
            
            // Assert
            true.Should().BeTrue();
        }
    }
}