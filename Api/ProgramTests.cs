I'll provide a comprehensive set of test case templates for different scenarios in a .NET Test project:

1. Controller Tests:
```csharp
using Xunit;
using Moq;
using MyApp.Controllers;
using MyApp.Services;
using Microsoft.AspNetCore.Mvc;

public class UserControllerTests
{
    [Fact]
    public void GetUser_ExistingUser_ReturnsOkResult()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(x => x.GetUserById(It.IsAny<int>()))
            .Returns(new UserDto());
        
        var controller = new UserController(mockUserService.Object);

        // Act
        var result = controller.GetUser(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void CreateUser_ValidInput_ReturnsCreatedResult()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var userDto = new CreateUserDto { Username = "t...