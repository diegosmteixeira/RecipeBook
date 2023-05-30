using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;

namespace TestsUtilities.Repositories;
public class RecipeReadOnlyRepositoryBuilder
{
    private static RecipeReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IRecipeReadOnlyRepository> _repository;
    private RecipeReadOnlyRepositoryBuilder()
    {
        if (_repository is null)
        {
            _repository = new Mock<IRecipeReadOnlyRepository>();
        }
    }

    public static RecipeReadOnlyRepositoryBuilder Instance()
    {
        _instance = new RecipeReadOnlyRepositoryBuilder();
        return _instance;
    }

    public IRecipeReadOnlyRepository Build()
    {
        return _repository.Object;
    }

    public RecipeReadOnlyRepositoryBuilder RecipesRecover(Recipe recipe)
    {
        if (recipe is not null)
        {
            _repository.Setup(r => r.RecipeRecovery(recipe.UserId)).ReturnsAsync(new List<Recipe> { recipe });
        }
        return this;
    }

    public RecipeReadOnlyRepositoryBuilder RecoverById(Recipe recipe)
    {
        _repository.Setup(repo => repo.RecipeRecoveryById(recipe.Id)).ReturnsAsync(recipe);
        return this;
    }
}
