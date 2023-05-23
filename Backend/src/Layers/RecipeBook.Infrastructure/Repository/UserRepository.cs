using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace RecipeBook.Infrastructure.Repository;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private RecipeBookContext _context { get; set; }
    public UserRepository(RecipeBookContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> CheckIfUserExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email.Equals(email));
    }

    public async Task<User> Login(string email, string password)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public async Task<User> UserRecoveryByEmail(string email)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> UserRecoveryById(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
