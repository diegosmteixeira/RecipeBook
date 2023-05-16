using FluentAssertions;
using RecipeBook.Communication.Request;
using RecipeBook.Exception;
using System.Net;
using System.Text.Json;
using TestsUtilities.Requests;
using Xunit;

namespace WebApi.Test.V1.User.Register;

public class UserRegisterTest : ControllerBase
{
    private const string METHOD = "api/user";
    public UserRegisterTest(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Validate_Success()
    {
        var request = RequestUserRegisterBuilder.Build();

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Empty_Name_Error()
    {
        var request = RequestUserRegisterBuilder.Build();
        request.Name = "";

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ResourceErrorMessages.EMPTY_USERNAME));
    }

}
