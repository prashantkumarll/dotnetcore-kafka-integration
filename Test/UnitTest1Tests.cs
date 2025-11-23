using System;
using Xunit;
using FluentAssertions;

namespace Test
{
    public class UnitTest1Tests
    {
        [Fact]
        public void Test1_DefaultScenario_Passes()
        {
            // Arrange
            var unitTest = new UnitTest1();

            // Act
            Action testAction = () => unitTest.Test1();

            // Assert
            testAction.Should().NotThrow();
        }

        [Fact]
        public void UnitTest1_Instantiation_ShouldCreateInstance()
        {
            // Arrange & Act
            var unitTest = new UnitTest1();

            // Assert
            unitTest.Should().NotBeNull();
        }
    }
}