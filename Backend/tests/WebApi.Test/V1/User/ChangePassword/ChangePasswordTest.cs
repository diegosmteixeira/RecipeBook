using FluentAssertions;
using RecipeBook.Exception;
using System.Net;
using System.Text.Json;
using TestsUtilities.Requests;
using Xunit;

namespace WebApi.Test.V1.User.ChangePassword;

public class ChangePasswordTest : ControllerBase
{
    private const string METHOD = "api/user/change-password";
    private RecipeBook.Domain.Entities.User _user;
    private string _password;
    public ChangePasswordTest(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
        _user = factory.UserRecovery();
        _password = factory.PasswordRecovery();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_user.Email, _password);

        var request = RequestChangePasswordUserBuilder.Build();
        request.CurrentPassword = _password;

        var response = await PutRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Validate_Empty_NewPassword_Error()
    {
        var token = await Login(_user.Email, _password);

        var request = RequestChangePasswordUserBuilder.Build();
        request.CurrentPassword = _password;
        request.NewPassword = string.Empty;

        var response = await PutRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ResourceErrorMessages.EMPTY_PASSWORD));

    }
}
