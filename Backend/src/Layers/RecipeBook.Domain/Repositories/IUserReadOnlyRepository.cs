namespace RecipeBook.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> CheckIfUserExists(string email);
}
