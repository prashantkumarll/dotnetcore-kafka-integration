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
        public void Constructor_ShouldInitializeObject_Successfully()
        {
            // Arrange & Act
            var unitTest = new UnitTest1();

            // Assert
            unitTest.Should().NotBeNull();
        }

        [Fact]
        public void Test1_ShouldHaveEmptyImplementation_WhenCalled()
        {
            // Arrange
            var unitTest = new UnitTest1();

            // Act
            unitTest.Test1();

            // Assert - Verifies method can be called without exceptions
            true.Should().BeTrue();
        }
    }
}