using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUserRegisterJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> UserRegister([FromServices] IUserRegisterUseCase useCase,
                                                  [FromBody] RequestUserRegisterJson request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }
}
