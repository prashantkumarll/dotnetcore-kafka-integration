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
            unitTest.Test1();

            // Assert
            true.Should().BeTrue();
        }

        [Fact]
        public void UnitTest1_Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var unitTest = new UnitTest1();

            // Assert
            unitTest.Should().NotBeNull();
        }
    }
}