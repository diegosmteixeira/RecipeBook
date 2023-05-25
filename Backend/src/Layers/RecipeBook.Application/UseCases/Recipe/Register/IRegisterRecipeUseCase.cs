using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.Recipe.Register;
public interface IRegisterRecipeUseCase
{
    Task<ResponseRecipeJson> Execute(RequestRecipeRegisterJson request);
}
