using Microsoft.AspNetCore.Http;
using RecipeBook.Application.Services.Token;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace RecipeBook.Application.Services.LoggedUser;

public class UserLogged : IUserLogged
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TokenConfigurator _tokenConfigurator;
    private readonly IUserReadOnlyRepository _userRepository;
    public UserLogged(IHttpContextAccessor contextAccessor,
                      TokenConfigurator tokenConfigurator,
                      IUserReadOnlyRepository userRepository)
    {
        _contextAccessor = contextAccessor;
        _tokenConfigurator = tokenConfigurator;
        _userRepository = userRepository;
    }
    public async Task<User> UserRecovery()
    {
        var headerToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

        var token = headerToken["Bearer".Length..].Trim();

        var userEmail = _tokenConfigurator.EmailRecovery(token);

        var user = await _userRepository.UserRecoveryByEmail(userEmail);

        return user;        
    }
}
