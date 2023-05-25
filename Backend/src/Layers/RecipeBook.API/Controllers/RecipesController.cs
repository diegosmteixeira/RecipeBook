using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Filters;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.API.Controllers;

[ServiceFilter(typeof(AuthorizationAttribute))]
public class RecipesController : RecipeBookController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRegisterRecipeUseCase useCase,
                                              [FromBody] RequestRecipeRegisterJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}
