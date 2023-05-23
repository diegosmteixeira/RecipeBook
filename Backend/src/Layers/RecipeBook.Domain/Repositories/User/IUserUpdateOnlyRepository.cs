namespace RecipeBook.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    void Update(Entities.User user);
    Task<Entities.User> UserRecoveryById(long id);
}
