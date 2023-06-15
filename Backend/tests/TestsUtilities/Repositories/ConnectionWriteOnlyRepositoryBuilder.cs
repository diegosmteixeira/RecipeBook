using Moq;
using RecipeBook.Domain.Repositories.Code;
using RecipeBook.Domain.Repositories.Connection;

namespace TestsUtilities.Repositories;
public class ConnectionWriteOnlyRepositoryBuilder
{
    private static ConnectionWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<IConnectionWriteOnlyRepository> _repository;

    private ConnectionWriteOnlyRepositoryBuilder()
    {
        if (_repository is null)
            _repository = new Mock<IConnectionWriteOnlyRepository>();
    }

    public static ConnectionWriteOnlyRepositoryBuilder Instance()
    {
        _instance = new ConnectionWriteOnlyRepositoryBuilder();
        return _instance;
    }

    public IConnectionWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}
