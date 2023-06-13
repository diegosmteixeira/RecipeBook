namespace RecipeBook.Domain.Repositories.Recipe;
public interface IRecipeReadOnlyRepository
{
    Task<IList<Entities.Recipe>> RecipeRecovery(long userId);
    Task<Entities.Recipe> RecipeRecoveryById(long recipeId);
    Task<int> RecipeRecoveryCount(long userId);
    Task<IList<Entities.Recipe>> RecipeRecoveryUsersConnectedWith(List<long> userIds);

}
