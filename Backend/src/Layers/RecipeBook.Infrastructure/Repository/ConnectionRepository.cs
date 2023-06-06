using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Connection;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace RecipeBook.Infrastructure.Repository;
public class ConnectionRepository : IConnectionWriteOnlyRepository, IConnectionReadOnlyRepository
{
    private readonly RecipeBookContext _recipeBookContext;

    public ConnectionRepository(RecipeBookContext recipeBookContext)
    {
        _recipeBookContext = recipeBookContext;
    }

    public async Task<bool> IsConnectedAsync(long userId, long otherUserId)
    {
        return await _recipeBookContext.Connections
            .AsNoTracking()
            .AnyAsync(c => c.UserId == userId && c.ConnectedWithUserId == otherUserId);
    }

    public async Task Register(Connection connection)
    {
        await _recipeBookContext.Connections.AddAsync(connection);
    }
}