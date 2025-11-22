using System;
using Xunit;
using FluentAssertions;

namespace Test
{
    public class UnitTest1Tests
    {
        [Fact]
        public void Test1_EmptyTest_ShouldPass()
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