using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RainCustomers.API.Controllers;
using RainCustomers.API.Models;
using RainCustomers.API.Services;
using RainCustomers.UnitTests.Fixtures;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RainCustomers.UnitTests.Systems.Controllers;

public class TestUsersController
{
    private readonly Mock<IUsersService> _usersServiceMock;

    public TestUsersController()
    {
        _usersServiceMock = new Mock<IUsersService>();
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        _usersServiceMock
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());

        var sut = new UsersController(_usersServiceMock.Object);

        // Act
        var result = await sut.Get();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;

        objectResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokesUserServiceExactlyOnce()
    {
        _usersServiceMock
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(_usersServiceMock.Object);

        var result = sut.Get();

        _usersServiceMock.Verify(
            service => service.GetAllUsers(),
            Times.Once()
        );
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfUsers()
    {

        _usersServiceMock
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());

        var sut = new UsersController(_usersServiceMock.Object);

        var result = await sut.Get();

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<User>>();
    }

    [Fact]
    public async Task Get_OnNoUsersFound_Returns404()
    {
        _usersServiceMock
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(_usersServiceMock.Object);
        var result = await sut.Get();

        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }

}
