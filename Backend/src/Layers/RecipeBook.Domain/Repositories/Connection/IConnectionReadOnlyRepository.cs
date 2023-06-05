namespace RecipeBook.Domain.Repositories.Connection;
public interface IConnectionReadOnlyRepository
{
    Task<bool> IsConnectedAsync(long userId , long otherUserId);
}
