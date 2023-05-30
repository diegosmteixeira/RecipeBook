using FluentAssertions;
using RecipeBook.Exception;
using System.Net;
using System.Text.Json;
using TestsUtilities.Hashids;
using Xunit;

namespace WebApi.Test.V1.Recipe.RecoverById;
public class RecipeRecoverByIdTest : ControllerBase
{
    private const string METHOD = "api/recipes";

    private RecipeBook.Domain.Entities.User _user;
    private string _password;
    public RecipeRecoverByIdTest(RecipeBookWebApplicationFactory<Program> factory) : base(factory)
    {
        _user = factory.UserRecovery();
        _password = factory.PasswordRecovery();
    }

    [Fact]
    public async Task Validate_Success()
    {
        // arrange
        var token = await Login(_user.Email, _password);

        var recipeId = await GetRecipeId(token);

        // act
        var response = await GetRequest($"{METHOD}/{recipeId}", token);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("category").GetUInt16().Should().BeInRange(0, 3);
        responseData.RootElement.GetProperty("instructions").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("ingredients").GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Validate_Recipe_Not_Exists_Failure()
    {
        // arrange
        var token = await Login(_user.Email, _password);

        var recipeId = HashidsBuilder.Instance().Build().EncodeLong(0);

        // act
        var response = await DeleteRequest($"{METHOD}/{recipeId}", token);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.RECIPE_NOTFOUND;
        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(expectedMessage));
    }
}
