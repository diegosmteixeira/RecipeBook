using Bogus.Bson;
using Bogus.DataSets;
using FluentAssertions;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using System.Text.Json;
using TestsUtilities.Requests;
using Xunit;

namespace WebApi.Test.V1.Recipe.Register;
public class RecipeRegisterTest : ControllerBase
{
    private const string METHOD = "api/recipes";

    private RecipeBook.Domain.Entities.User _user;
    private string _password;

    public RecipeRegisterTest(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
        _user = factory.UserRecovery();
        _password = factory.PasswordRecovery();
    }

    [Fact]
    public async Task Validate_Success()
    {
        // arrange
        var token = await Login(_user.Email, _password);

        var request = RequestRecipeRegisterBuilder.Build();

        //act
        var response = await PostRequest(METHOD, request, token);

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
        responseData.RootElement.GetProperty("category").GetUInt16().Should().Be((ushort)request.Category);
        responseData.RootElement.GetProperty("instructions").GetString().Should().Be(request.Instructions);
    }

    [Fact]
    public async Task Validate_Empty_Ingredients_Failure()
    {
        // arrange
        var token = await Login(_user.Email, _password);

        var request = RequestRecipeRegisterBuilder.Build();
        request.Ingredients.Clear();

        //act
        var response = await PostRequest(METHOD, request, token);

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ResourceErrorMessages.EMPTY_INGREDIENTS));
    }
}
