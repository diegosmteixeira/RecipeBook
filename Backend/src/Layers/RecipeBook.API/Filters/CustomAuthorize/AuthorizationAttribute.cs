using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.Application.Services.Token;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.API.Filters.CustomAuthorize;

public class AuthorizationAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly TokenConfigurator _tokenConfigurator;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    public AuthorizationAttribute(TokenConfigurator token,
                                  IUserReadOnlyRepository user)
    {
        _tokenConfigurator = token;
        _userReadOnlyRepository = user;
    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenInsideRequest(context);
            var email = _tokenConfigurator.EmailRecovery(token);

            var user = await _userReadOnlyRepository.UserRecoveryByEmail(email);

            if (user is null)
            {
                throw new RecipeBookException(string.Empty);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            TokenExpired(context);
        }
        catch
        {
            UserWithoutPermission(context);
        }
    }

    private static string TokenInsideRequest(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authorization))
        {
            throw new RecipeBookException(string.Empty);
        }
        return authorization["Bearer".Length..].Trim();
    }

    private static void TokenExpired(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(ResourceErrorMessages.TOKEN_EXPIRED));
    }

    private static void UserWithoutPermission(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(ResourceErrorMessages.WITHOUT_PERMISSION));
    }
}
