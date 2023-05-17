using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.User.Login;

public interface IUserLoginUseCase
{
    Task<ResponseLoginJson> Execute(RequestLoginJson request);
}
