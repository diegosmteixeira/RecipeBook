using FluentAssertions;
using Newtonsoft.Json;
using RecipeBook.Exception;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1.UnknownError;
public class LoginUnknownErrorTest : IClassFixture<RecipeBookWebApplicationFactoryWithoutDependencyInjection<Program>>
{
    private const string METHOD = "api/login";
    private readonly HttpClient _httpClient;

    public LoginUnknownErrorTest(RecipeBookWebApplicationFactoryWithoutDependencyInjection<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("pt")]
    [InlineData("en")]
    public async Task Validate_Unknown_Error_Failure(string culture)
    {
        var request = new RecipeBook.Communication.Request.RequestLoginJson();

        var response = await PostRequest(METHOD, request, culture);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var message = ResourceErrorMessages.ResourceManager.GetString("UNKNOWN_ERROR", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(message));
    }

    private async Task<HttpResponseMessage> PostRequest(string method, object body, string culture)
    {
        ChangeCultureRequest(culture);

        var jsonString = JsonConvert.SerializeObject(body);
        return await _httpClient.PostAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    private void ChangeCultureRequest(string culture)
    {
        if (!string.IsNullOrWhiteSpace(culture))
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            }

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        }
    }
}