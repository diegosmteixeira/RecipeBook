using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace RecipeBook.Infrastructure.Repository;
public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepositoy
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

    public async Task<IList<Recipe>> RecipeRecovery(long userId)
    {
        return await _context.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<Recipe> RecipeRecoveryById(long recipeId)
    {
        return await _context.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .Where(r => recipeId == r.Id).FirstOrDefaultAsync();
    }
}
