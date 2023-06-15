using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace RecipeBook.Infrastructure.Repository;
public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
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

    public async Task<IList<Recipe>> RecipeRecoveryUsersConnectedWith(List<long> userIds)
    {
        return await _context.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .Where(r => userIds.Contains(r.UserId)).ToListAsync();
    }

    async Task<Recipe> IRecipeReadOnlyRepository.RecipeRecoveryById(long recipeId)
    {
        return await _context.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => recipeId == r.Id);
    }

    async Task<Recipe> IRecipeUpdateOnlyRepository.RecipeRecoveryById(long recipeId)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => recipeId == r.Id);
    }

    public void Update(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
    }

    public async Task Delete(long recipeId)
    {
        var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

        _context.Recipes.Remove(recipe);
    }

    public async Task<int> RecipeRecoveryCount(long userId)
    {
        return await _context.Recipes.CountAsync(r => r.UserId == userId);
    }
}
