using RecipeBook.Communication.Request;

namespace RecipeBook.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
