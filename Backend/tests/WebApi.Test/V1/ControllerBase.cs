using Newtonsoft.Json;
using RecipeBook.Exception;
using System.Globalization;
using System.Text;
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

        protected async Task<HttpResponseMessage> PostRequest(string method, object body)
        {
            var jsonString = JsonConvert.SerializeObject(body);
            return await _httpClient.PostAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }
    }
}
