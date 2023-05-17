namespace RecipeBook.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> CheckIfUserExists(string email);
    Task<Entities.User> Login (string email, string password);
}
