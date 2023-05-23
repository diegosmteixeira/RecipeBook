using Moq;
using RecipeBook.Domain.Repositories.User;

namespace TestsUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private static UserUpdateOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserUpdateOnlyRepository> _repository;

    private UserUpdateOnlyRepositoryBuilder()
    {
        if (_repository == null)
        {
            _repository = new Mock<IUserUpdateOnlyRepository>();
        }
    }

    public static UserUpdateOnlyRepositoryBuilder Instance()
    {
        _instance = new UserUpdateOnlyRepositoryBuilder();
        return _instance;
    }

    public UserUpdateOnlyRepositoryBuilder UserRecoveryById(RecipeBook.Domain.Entities.User user)
    {
        _repository.Setup(c => c.UserRecoveryById(user.Id)).ReturnsAsync(user);
        return this;
    }

    public IUserUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }
}
