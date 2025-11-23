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
            var unitTest = new UnitTest1();

            // Act
            Action testAction = () => unitTest.Test1();

            // Assert
            testAction.Should().NotThrow();
        }

        [Fact]
        public void UnitTest1_Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            Action constructorAction = () => new UnitTest1();

            // Assert
            constructorAction.Should().NotThrow();
        }

        [Fact]
        public void Test1_MultipleInvocations_ShouldRemainStable()
        {
            // Arrange
            var unitTest = new UnitTest1();

            // Act & Assert
            for (int i = 0; i < 5; i++)
            {
                Action testAction = () => unitTest.Test1();
                testAction.Should().NotThrow();
            }
        }
    }
}