using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Exception;

namespace RecipeBook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] IUserRegisterUseCase useCase)
    {
        var response = await useCase.Execute(new Communication.Request.RequestUserRegisterJson
        {
            Email = "email@email.com",
            Name = "Test",
            Password = "password",
            Phone = "12 3 4567-8900"

        });

        return Ok(response);
    }
}
