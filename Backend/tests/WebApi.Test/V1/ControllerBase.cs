using Newtonsoft.Json;
using RecipeBook.Communication.Request;
using RecipeBook.Exception;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1
{
    public class ControllerBase : IClassFixture<RecipeBookWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public ControllerBase(RecipeBookWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
            ResourceErrorMessages.Culture = CultureInfo.CurrentCulture;  
        }

        protected async Task<HttpResponseMessage> GetRequest(string method, string token = "")
        {
            AuthorizeRequest(token);

            return await _httpClient.GetAsync(method);
        }

        protected async Task<string> GetRecipeId(string token)
        {
            var request = new RequestDashboardJson();

            var response = await PutRequest("api/dashboard", request, token);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            return responseData.RootElement.GetProperty("recipes").EnumerateArray().First().GetProperty("id").GetString();
        }

        protected async Task<HttpResponseMessage> PostRequest(string method, object body, string token = "")
        {
            AuthorizeRequest(token);
            var jsonString = JsonConvert.SerializeObject(body);
            return await _httpClient.PostAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }

        protected async Task<HttpResponseMessage> PutRequest(string method, object body, string token = "")
        {
            AuthorizeRequest(token);

            var jsonString = JsonConvert.SerializeObject(body);
            return await _httpClient.PutAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }

        protected async Task<HttpResponseMessage> DeleteRequest(string method, string token = "")
        {
            AuthorizeRequest(token);

            return await _httpClient.DeleteAsync(method);
        }


        protected async Task<string> Login(string email, string password)
        {
            var request = new RecipeBook.Communication.Request.RequestLoginJson
            {
                Email = email,
                Password = password
            };

            var response = await PostRequest("api/login", request);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            return responseData.RootElement.GetProperty("token").GetString();
        }

        private void AuthorizeRequest(string token)
        {
            if (!string.IsNullOrWhiteSpace(token) && !_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }
    }
}
