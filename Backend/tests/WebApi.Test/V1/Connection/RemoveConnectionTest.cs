using FluentAssertions;
using RecipeBook.Exception;
using System.Net;
using System.Text.Json;
using TestsUtilities.Hashids;
using Xunit;

namespace WebApi.Test.V1.Connection;
public class RemoveConnectionTest : ControllerBase
{
    private const string METHOD = "api/connections";

    private RecipeBook.Domain.Entities.User _userWithoutConnection;
    private string _passwordWithoutConnection;

    private RecipeBook.Domain.Entities.User _userWithConnection;
    private string _passwordWithConnection;

    public RemoveConnectionTest(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
        _userWithoutConnection = factory.UserRecovery();
        _passwordWithoutConnection = factory.PasswordRecovery();

        _userWithConnection = factory.UserRecoveryWithConnection();
        _passwordWithConnection = factory.PasswordRecoveryWithConnection();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_userWithConnection.Email, _passwordWithConnection);

        // get all connections
        var response = await GetRequest(METHOD, token);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var users = responseData.RootElement.GetProperty("users").EnumerateArray();

        var idToRemove = users.First().GetProperty("id").GetString();
        
        // delete response
        var deleteResponse = await DeleteRequest($"{METHOD}/{idToRemove}", token);

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // response after delete for check
        // expected result = NoContent (this user only have one connection)
        var afterDeleteResponse = await GetRequest(METHOD, token);

        afterDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Validate_User_Without_Connection_Failure()
    {
        var token = await Login(_userWithoutConnection.Email, _passwordWithoutConnection);

        var idToRemove = HashidsBuilder.Instance().Build().EncodeLong(0);

        var deleteResponse = await DeleteRequest($"{METHOD}/{idToRemove}", token);

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await deleteResponse.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ResourceErrorMessages.USER_NOT_FOUND));
    }

    [Fact]
    public async Task Validate_User_Id_Failure()
    {
        var token = await Login(_userWithConnection.Email, _passwordWithConnection);

        var idToRemove = HashidsBuilder.Instance().Build().EncodeLong(0);

        var deleteResponse = await DeleteRequest($"{METHOD}/{idToRemove}", token);

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await deleteResponse.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ResourceErrorMessages.USER_NOT_FOUND));
    }
}
