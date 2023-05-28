using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.Recipe.Recover;
public interface IRecipeRecoveryByIdUseCase
{
    Task<ResponseRecipeJson> Execute(long id);
}
