using System;
using Xunit;
using FluentAssertions;

namespace Api.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Program_Main_ShouldRunWithoutErrors()
        {
            // Arrange
            Action runProgram = () => Program.Main(new string[] {});

            // Act & Assert
            runProgram.Should().NotThrow();
        }
    }
}