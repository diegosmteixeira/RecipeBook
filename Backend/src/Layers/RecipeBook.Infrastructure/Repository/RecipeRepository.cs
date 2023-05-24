using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace RecipeBook.Infrastructure.Repository;
public class RecipeRepository : IRecipeWriteOnlyRepository
{
    private readonly RecipeBookContext _context;
    public RecipeRepository(RecipeBookContext context)
    {
        _context = context;
    }
    public async Task AddRecipe(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
    }
}
