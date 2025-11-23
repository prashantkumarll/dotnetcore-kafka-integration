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
            unitTest.Test1();

            // Assert
            true.Should().BeTrue();
        }

        [Fact]
        public void UnitTest1_Constructor_Initializes()
        {
            // Arrange & Act
            var unitTest = new UnitTest1();

            // Assert
            unitTest.Should().NotBeNull();
        }

        [Fact]
        public void Test1_EmptyMethod_DoesNotThrow()
        {
            // Arrange
            var unitTest = new UnitTest1();

            // Act
            Action testAction = () => unitTest.Test1();

            // Assert
            testAction.Should().NotThrow();
        }
    }
}