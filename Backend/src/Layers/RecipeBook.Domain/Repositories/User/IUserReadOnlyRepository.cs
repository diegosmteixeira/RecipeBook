namespace RecipeBook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> CheckIfUserExists(string email);
    Task<Entities.User> UserRecoveryByEmail(string email);
    Task<Entities.User> Login(string email, string password);
}
