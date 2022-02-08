using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using RainCustomers.API.Config;
using RainCustomers.API.Models;
using RainCustomers.API.Services;
using RainCustomers.UnitTests.Fixtures;
using RainCustomers.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RainCustomers.UnitTests.Systems.Services;

public class TestUsersService
{
    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokeHttpGetRequest()
    {
        // Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = "http://example.com"
        });
        var sut = new UsersService(httpClient, config);

        // Act
        await sut.GetAllUsers();

        // Assert
        handlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );

    }

    [Fact]
    public async Task GetAllUsers_WhenHits404_ReturnsEmptyListOfUsers()
    {
        // Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();
        var httpClient = new HttpClient(handlerMock.Object);
        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = "http://example.com"
        });
        var sut = new UsersService(httpClient, config);

        // Act
        var result = await sut.GetAllUsers();

        // Assert
        result.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetAllUsers_WhenCalled_ReturnsListOfUsersOfExpectedSize()
    {
        // Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = "http://example.com"
        });
        var sut = new UsersService(httpClient, config);

        // Act
        var result = await sut.GetAllUsers();

        // Assert
        result.Count.Should().Be(expectedResponse.Count);
    }


    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
    {
        // Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "https://example.com/users";
        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });
        var sut = new UsersService(httpClient, config);

        // Act
        var result = await sut.GetAllUsers();

        // Assert
        var uri = new Uri(endpoint);
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(
                req => req.Method == HttpMethod.Get
                && req.RequestUri == uri),
            ItExpr.IsAny<CancellationToken>()
            );
    }
}

