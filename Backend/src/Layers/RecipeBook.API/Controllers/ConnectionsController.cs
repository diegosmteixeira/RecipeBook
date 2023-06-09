using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Filters.CustomAuthorize;
using RecipeBook.Application.UseCases.Connection.RecoverConnection;
using RecipeBook.Communication.Response;

namespace RecipeBook.API.Controllers;


public class ConnectionsController : RecipeBookController
{
    [HttpGet]
    [ProducesResponseType(typeof(IList<ResponseUserConnectedWithJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(AuthorizationAttribute))]
    public async Task<IActionResult> RecoverConnections([FromServices] IRecoverConnectionUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Any())
            return Ok(response);

        return NoContent();
    }
}
