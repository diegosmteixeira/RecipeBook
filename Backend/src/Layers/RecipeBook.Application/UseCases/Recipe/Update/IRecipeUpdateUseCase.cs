using RecipeBook.Communication.Request;

namespace RecipeBook.Application.UseCases.Recipe.Update;
public interface IRecipeUpdateUseCase
{
    Task Execute(long id, RequestRecipeJson request);
}
