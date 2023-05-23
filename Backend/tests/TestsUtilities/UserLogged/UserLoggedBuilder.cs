using Moq;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Domain.Repositories.User;
using TestsUtilities.Repositories;

namespace TestsUtilities.UserLogged;

public class UserLoggedBuilder
{
    private static UserLoggedBuilder _instance;
    private readonly Mock<IUserLogged> _repository;

    private UserLoggedBuilder()
    {
        if (_repository == null)
        {
            _repository = new Mock<IUserLogged>();
        }
    }

    public static UserLoggedBuilder Instance()
    {
        _instance = new UserLoggedBuilder();
        return _instance;
    }

    public UserLoggedBuilder UserRecovery(RecipeBook.Domain.Entities.User user)
    {
        _repository.Setup(c => c.UserRecovery()).ReturnsAsync(user);
        return this;
    }

    public IUserLogged Build()
    {
        return _repository.Object;
    }
}
