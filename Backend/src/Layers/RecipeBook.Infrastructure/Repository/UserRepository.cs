using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace RecipeBook.Infrastructure.Repository;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private RecipeBookContext _context { get; set; }
    public UserRepository(RecipeBookContext context)
    {
        _context = context;
    }

    public RecipeBookContext Context { get; }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> CheckIfUserExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email.Equals(email));
    }
}
