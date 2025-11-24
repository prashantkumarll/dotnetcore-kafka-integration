using System;
using Xunit;
using FluentAssertions;

namespace Test
{
    public class UnitTest1Tests
    {
        [Fact]
        public void Test1_ShouldPass_WhenCalled()
        {
            // Arrange
            var unitTest = new UnitTest1();

            // Act
            Action testAction = () => unitTest.Test1();

            // Assert
            testAction.Should().NotThrow();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithoutErrors()
        {
            // Arrange & Act
            Action constructorAction = () => new UnitTest1();

            // Assert
            constructorAction.Should().NotThrow();
        }
    }
}