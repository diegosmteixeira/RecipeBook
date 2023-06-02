using Microsoft.AspNetCore.Authorization;
using RecipeBook.Application.Services.Token;
using RecipeBook.Domain.Repositories.User;

namespace RecipeBook.API.Filters.CustomAuthorize;

public class UserLoggedHandler : AuthorizationHandler<UserLoggedRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenConfigurator _tokenConfigurator;
    private readonly IUserReadOnlyRepository _repository;

    public UserLoggedHandler(IHttpContextAccessor httpContextAccessor, 
                             TokenConfigurator tokenConfigurator, 
                             IUserReadOnlyRepository repository)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenConfigurator = tokenConfigurator;
        _repository = repository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserLoggedRequirement requirement)
    {
        try
        {
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            var token = authorization["Bearer".Length..].Trim();

            var userEmail = _tokenConfigurator.EmailRecovery(token);

            var user = await _repository.UserRecoveryByEmail(userEmail);

            if (user is null || string.IsNullOrWhiteSpace(authorization))
            {
                context.Fail();
                return;
            }
            else
            {
                context.Succeed(requirement);
            }
        }
        catch
        {
            context.Fail();
        }
    }
}