namespace RecipeBook.Domain.Repositories.Connection;
public interface IConnectionReadOnlyRepository
{
    Task<bool> IsConnectedAsync(long userId , long otherUserId);
    Task<IList<Entities.User>> RecoverConnections(long userId);
}
