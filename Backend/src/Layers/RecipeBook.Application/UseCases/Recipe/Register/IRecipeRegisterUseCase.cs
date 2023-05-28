using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.Recipe.Register;
public interface IRecipeRegisterUseCase
{
    Task<ResponseRecipeJson> Execute(RequestRecipeJson request);
}
