using RecipeBook.Domain.Repositories;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace RecipeBook.Infrastructure.Repository;

public sealed class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly RecipeBookContext _context;
    private bool _disposed;
    public UnitOfWork(RecipeBookContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }
}
