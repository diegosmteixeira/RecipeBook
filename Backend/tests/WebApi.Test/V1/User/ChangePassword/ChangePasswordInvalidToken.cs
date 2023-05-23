using FluentAssertions;
using System.Net;
using TestsUtilities.Requests;
using TestsUtilities.Token;
using Xunit;

namespace WebApi.Test.V1.User.ChangePassword;

public class ChangePasswordInvalidToken : ControllerBase
{
    private const string METHOD = "api/user/change-password";
    private RecipeBook.Domain.Entities.User _user;
    private string _password;
    public ChangePasswordInvalidToken(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
        _user = factory.UserRecovery();
        _password = factory.PasswordRecovery();
    }

    [Fact]
    public async Task Validate_Empty_Token_Error()
    {
        var token = string.Empty;

        var request = RequestChangePasswordUserBuilder.Build();
        request.CurrentPassword = _password;

        var response = await PutRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_Token_Fake_Email_Error()
    {
        var token = TokenConfiguratorBuilder.Instance().GenerateToken("user@fake.com");

        var request = RequestChangePasswordUserBuilder.Build();
        request.CurrentPassword = _password;

        var response = await PutRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_Expired_Token_Error()
    {
        var token = TokenConfiguratorBuilder.ExpiredToken().GenerateToken(_user.Email);
        Thread.Sleep(1000);

        var request = RequestChangePasswordUserBuilder.Build();
        request.CurrentPassword = _password;

        var response = await PutRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

}
