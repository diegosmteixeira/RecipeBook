namespace RecipeBook.Application.UseCases.Recipe.Delete;
public interface IRecipeDeleteUseCase
{
    Task Execute(long id);
}
