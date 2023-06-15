using FluentAssertions;
using RecipeBook.Application.UseCases.Connection.AcceptConection;
using TestsUtilities.Entities;
using TestsUtilities.Hashids;
using TestsUtilities.Repositories;
using TestsUtilities.UserLogged;
using Xunit;

namespace UseCases.Test.Connection;
public class AcceptConnectionUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        (var userToConnect, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(userToConnect);

        var hashids = HashidsBuilder.Instance().Build();

        var result = await useCase.Execute(hashids.EncodeLong(userToConnect.Id));

        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Be(hashids.EncodeLong(userToConnect.Id));
    }

    private static AcceptConnectionUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
    {
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var writeRepository = CodeWriteOnlyRepositoryBuilder.Instance().Build();
        var writeConnection = ConnectionWriteOnlyRepositoryBuilder.Instance().Build();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var hashids = HashidsBuilder.Instance().Build();

        return new AcceptConnectionUseCase(writeConnection, writeRepository, userLogged, unitOfWork, hashids);
    }
}
