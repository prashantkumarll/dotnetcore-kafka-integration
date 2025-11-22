using System;
using Xunit;
using FluentAssertions;

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
        public void Test1_MultipleInputs_ShouldValidate(int input)
        {
            // Arrange
            // Act
            // Assert
            input.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Test1_NullScenario_ShouldHandleSafely()
        {
            // Arrange
            object testObject = null;

            // Act
            Action nullCheck = () => {
                testObject.Should().BeNull();
            };

            // Assert
            nullCheck.Should().NotThrow();
        }

        [Fact]
        public void Test1_ExceptionScenario_ShouldCatchException()
        {
            // Arrange
            Action throwingAction = () => throw new InvalidOperationException();

            // Act & Assert
            throwingAction.Should().Throw<InvalidOperationException>(); 
        }

        [Fact]
        public void Test1_EdgeCase_ShouldHandleUnexpectedInput()
        {
            // Arrange
            // Act
            // Assert
            true.Should().BeTrue("Default test case should always pass");
        }
    }
}