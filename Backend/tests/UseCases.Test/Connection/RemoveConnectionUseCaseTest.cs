using FluentAssertions;
using RecipeBook.Application.UseCases.Connection.RemoveConnection;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using TestsUtilities.Entities;
using TestsUtilities.Repositories;
using TestsUtilities.UserLogged;
using UseCases.Test.Connection.InlineData;
using Xunit;

namespace UseCases.Test.Connection;
public class RemoveConnectionUseCaseTest
{
    [Theory]
    [ClassData(typeof(UserEntitiesConnectionDataTest))]
    public async Task Validate_Success(long userIdToRemove, IList<RecipeBook.Domain.Entities.User> connections)
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(connections, user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(userIdToRemove);
        };

        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Validate_Invalid_User_Failure()
    {
        (var user, var _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build(); 

        var useCase = CreateUseCase(connections, user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(0);
        };

        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(ex => ex.ErrorMessages.Count == 1 && ex.ErrorMessages.Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }

    private static RemoveConnectionUseCase CreateUseCase(IList<RecipeBook.Domain.Entities.User> connections,
                                                          RecipeBook.Domain.Entities.User user)
    {
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var connReadRepository = ConnectionReadOnlyRepositoryBuilder.Instance().RecoverConnections(user, connections).Build();
        var connWriteRepository = ConnectionWriteOnlyRepositoryBuilder.Instance().Build();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();

        return new RemoveConnectionUseCase(connReadRepository, connWriteRepository, userLogged, unitOfWork);
    }
}
