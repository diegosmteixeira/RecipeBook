using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;

namespace TestsUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private static UserReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserReadOnlyRepository> _repository;

    private UserReadOnlyRepositoryBuilder()
    {
        if (_repository == null)
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }
    }

    public static UserReadOnlyRepositoryBuilder Instance()
    {
        _instance = new UserReadOnlyRepositoryBuilder();
        return _instance;
    }

    public UserReadOnlyRepositoryBuilder CheckIfUserExists(string email)
    {
        if (!string.IsNullOrEmpty(email))
            _repository.Setup(i => i.CheckIfUserExists(email)).ReturnsAsync(true);

        return this;
    }

    public UserReadOnlyRepositoryBuilder Login(User user)
    {
        _repository.Setup(u => u.Login(user.Email, user.Password)).ReturnsAsync(user);
        return this;
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
