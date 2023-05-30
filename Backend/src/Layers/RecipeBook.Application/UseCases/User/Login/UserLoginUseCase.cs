using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.Token;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Login;

public class UserLoginUseCase : IUserLoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnly;
    private readonly Encryption _encryption;
    private readonly TokenConfigurator _tokenConfigurator;

    public UserLoginUseCase(IUserReadOnlyRepository userReadOnly,
                            TokenConfigurator tokenConfigurator,
                            Encryption encryption)
    {
        _userReadOnly = userReadOnly;
        _tokenConfigurator = tokenConfigurator;
        _encryption = encryption;
    }
    public async Task<ResponseLoginJson> Execute(RequestLoginJson request)
    {
        var encryptedPassword = _encryption.PasswordEncryption(request.Password);
        
        var user = await _userReadOnly.Login(request.Email, encryptedPassword);

        if (user is null)
        {
            throw new InvalidLoginException();
        }

        return new ResponseLoginJson
        {
            Name = user.Name,
            Token = _tokenConfigurator.GenerateToken(user.Email)
        };
    }
}
