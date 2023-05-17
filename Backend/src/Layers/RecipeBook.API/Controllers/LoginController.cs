using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.User.Login;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.API.Controllers
{
    public class LoginController : RecipeBookController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseLoginJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] RequestLoginJson request,
                                               [FromServices] IUserLoginUseCase useCase)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
