using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Connection;

namespace TestsUtilities.Repositories;
public class ConnectionReadOnlyRepositoryBuilder
{
    private static ConnectionReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IConnectionReadOnlyRepository> _repository;

    private ConnectionReadOnlyRepositoryBuilder()
    {
        if (_repository is null)
            _repository = new Mock<IConnectionReadOnlyRepository>();
    }

    public static ConnectionReadOnlyRepositoryBuilder Instance()
    {
        _instance = new ConnectionReadOnlyRepositoryBuilder();
        return _instance;
    }

    public ConnectionReadOnlyRepositoryBuilder IsConnectedAsync(long? userId, long? otherUserId)
    {
        if (userId.HasValue && otherUserId.HasValue)
        {
            _repository.Setup(x => x.IsConnectedAsync(userId.Value, otherUserId.Value)).ReturnsAsync(true);
        }

        return this;
    }

    public IConnectionReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
