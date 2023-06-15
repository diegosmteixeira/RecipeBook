using FluentAssertions;
using RecipeBook.Exception;
using System.Net;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1.User.Login;

public class LoginTest : ControllerBase
{
    private const string METHOD = "api/login";
    private RecipeBook.Domain.Entities.User _user;
    private string _password;
    public LoginTest(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
        _user = factory.UserRecovery();
        _password = factory.PasswordRecovery();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var request = new RecipeBook.Communication.Request.RequestLoginJson
        {
            Email = _user.Email, Password = _password
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_user.Name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Invalid_Password_Error()
    {
        var request = new RecipeBook.Communication.Request.RequestLoginJson
        {
            Email = _user.Email,
            Password = "invalid-Password"
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.Contain(ResourceErrorMessages.INVALID_LOGIN);
    }

    [Fact]
    public async Task Validate_Invalid_Email_Error()
    {
        var request = new RecipeBook.Communication.Request.RequestLoginJson
        {
            Email = "invalid-Email",
            Password = _password
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.Contain(ResourceErrorMessages.INVALID_LOGIN);
    }

    [Fact]
    public async Task Validate_Invalid_Email_And_Password_Error()
    {
        var request = new RecipeBook.Communication.Request.RequestLoginJson
        {
            Email = "invalid-Email",
            Password = "invalid-Password"
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.Contain(ResourceErrorMessages.INVALID_LOGIN);
    }
}
