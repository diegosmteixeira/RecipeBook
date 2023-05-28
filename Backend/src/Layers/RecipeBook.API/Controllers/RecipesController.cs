using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Filters;
using RecipeBook.Application.UseCases.Recipe.Recover;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Application.UseCases.Recipe.Update;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.API.Controllers;

[ServiceFilter(typeof(AuthorizationAttribute))]
public class RecipesController : RecipeBookController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRecipeRegisterUseCase useCase,
                                              [FromBody] RequestRecipeJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }

    [HttpGet]
    [Route("{id:hashids}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverById([FromServices] IRecipeRecoveryByIdUseCase useCase,
                                                 [FromRoute] [ModelBinder(typeof(Binder.HashidsModelBinder))] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpPut]
    [Route("{id:hashids}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromBody] RequestRecipeJson request,
                                            [FromServices] IRecipeUpdateUseCase useCase,
                                            [FromRoute] [ModelBinder(typeof(Binder.HashidsModelBinder))] long id)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
}
