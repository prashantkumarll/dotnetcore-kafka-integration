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
            
            // Act
            
            // Assert
            true.Should().BeTrue();
        }
    }
}