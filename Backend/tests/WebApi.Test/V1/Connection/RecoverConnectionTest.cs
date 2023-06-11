using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1.Connection;
public class RecoverConnectionTest : ControllerBase
{
    private const string METHOD = "api/connections";

    private RecipeBook.Domain.Entities.User _userWithoutConnection;
    private string _password;
    
    private RecipeBook.Domain.Entities.User _userWithConnection;
    private string _passwordWithConnection;

    public RecoverConnectionTest(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
        _userWithoutConnection = factory.UserRecovery();
        _password = factory.PasswordRecovery();

        _userWithConnection = factory.UserRecoveryWithConnection();
        _passwordWithConnection = factory.PasswordRecoveryWithConnection();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_userWithConnection.Email, _passwordWithConnection);

        var response = await GetRequest(METHOD, token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("users").GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Validate_User_Without_Connection_Failure()
    {
        var token = await Login(_userWithoutConnection.Email, _password);

        var response = await GetRequest(METHOD, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
