using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Filters.CustomAuthorize;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Application.UseCases.User.Profile;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.API.Controllers;

public class UserController : RecipeBookController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUserRegisterJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> UserRegister([FromServices] IUserRegisterUseCase useCase,
                                                  [FromBody] RequestUserRegisterJson request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpPut]
    [Route("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(AuthorizationAttribute))]
    public async Task<IActionResult> ChangePassword([FromServices] IChangePasswordUseCase useCase,
                                                        [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();

    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [ServiceFilter(typeof(AuthorizationAttribute))]
    public async Task<IActionResult> RecoverProfile([FromServices] IProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }
}
