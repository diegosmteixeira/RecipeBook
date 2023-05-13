using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.User.Register;

public interface IUserRegisterUseCase
{
    Task<ResponseUserRegisterJson> Execute(RequestUserRegisterJson request);
}
