namespace RecipeBook.Domain.Repositories.Recipe;
public interface IRecipeUpdateOnlyRepository
{
    Task<Entities.Recipe> RecipeRecoveryById(long recipeId);
    void Update(Entities.Recipe recipe);
}
