using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Binder;
using RecipeBook.API.Filters.CustomAuthorize;
using RecipeBook.Application.UseCases.Connection.RecoverConnection;
using RecipeBook.Application.UseCases.Connection.RemoveConnection;
using RecipeBook.Communication.Response;

namespace RecipeBook.API.Controllers;


public class ConnectionsController : RecipeBookController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserConnectedListJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecoverConnections([FromServices] IRecoverConnectionUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Users.Any())
            return Ok(response);

        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id:hashids}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveConnections([FromServices] IRemoveConnectionUseCase useCase,
                                                       [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
