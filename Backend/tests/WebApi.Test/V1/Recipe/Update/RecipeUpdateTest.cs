using FluentAssertions;
using RecipeBook.Exception;
using System.Net;
using System.Text.Json;
using TestsUtilities.Requests;
using Xunit;

namespace WebApi.Test.V1.Recipe.Update;
public class RecipeUpdateTest : ControllerBase
{
    private const string METHOD = "api/recipes";

    private RecipeBook.Domain.Entities.User _user;
    private string _password;
    public RecipeUpdateTest(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
        _user = factory.UserRecovery();
        _password = factory.PasswordRecovery();
    }

    [Fact]
    public async Task Validate_Success()
    {
        // arrange
        var token = await Login(_user.Email, _password);
        var request = RequestRecipeBuilder.Build();
        var recipeId = await GetRecipeId(token);

        // act
        var response = await PutRequest($"{METHOD}/{recipeId}", request, token);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // act
        var responseData = await GetRecipeById(token, recipeId);

        // assert again
        responseData.RootElement.GetProperty("id").GetString().Should().Be(recipeId);
        responseData.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
        responseData.RootElement.GetProperty("category").GetUInt16().Should().Be((ushort)request.Category);
        responseData.RootElement.GetProperty("instructions").GetString().Should().Be(request.Instructions);
    }

    [Fact]
    public async Task Validate_Empty_Ingredients_Failure()
    {
        // arrange
        var token = await Login(_user.Email, _password);
        var request = RequestRecipeBuilder.Build();
        request.Ingredients.Clear();

        var recipeId = await GetRecipeId(token);

        // act
        var response = await PutRequest($"{METHOD}/{recipeId}", request, token);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.EMPTY_INGREDIENTS;
        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(expectedMessage));
    }

    private async Task<JsonDocument> GetRecipeById(string token, string recipeId)
    {
        var response = await GetRequest($"{METHOD}/{recipeId}", token);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        return await JsonDocument.ParseAsync(responseBody);
    }

}
