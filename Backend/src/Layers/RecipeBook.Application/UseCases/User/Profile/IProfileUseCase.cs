using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.User.Profile;
public interface IProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
