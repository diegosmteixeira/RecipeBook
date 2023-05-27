using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Filters;
using RecipeBook.Application.UseCases.Dashboard;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.API.Controllers;


public class DashboardController : RecipeBookController
{
    [HttpPut]
    [ProducesResponseType(typeof(ResponseDashboardJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(AuthorizationAttribute))]
    public async Task<IActionResult> DashboardRecovery([FromServices] IDashboardUseCase useCase,
                                                       [FromBody] RequestDashboardJson request)
    {
        var response = await useCase.Execute(request);

        if (response.Recipes.Any())
            return Ok(response);

        return NoContent();
    }
}
