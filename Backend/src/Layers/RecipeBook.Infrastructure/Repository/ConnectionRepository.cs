﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<IList<User>> RecoverConnections(long userId)
    {
        return await _recipeBookContext.Connections.AsNoTracking()
            .Include(c => c.ConnectedWithUser)
            .Where(c => c.UserId == userId)
            .Select(c => c.ConnectedWithUser)
            .ToListAsync();
    }

    public async Task Register(Connection connection)
    {
        await _recipeBookContext.Connections.AddAsync(connection);
    }

    public async Task RemoveConnection(long userId, long userIdToRemove)
    {
        var connections = await _recipeBookContext.Connections
            .Where(c => (c.UserId == userId && c.ConnectedWithUserId == userIdToRemove) 
                     || (c.UserId == userIdToRemove && c.ConnectedWithUserId == userId))
            .ToListAsync();

        _recipeBookContext.RemoveRange(connections);
    }
}