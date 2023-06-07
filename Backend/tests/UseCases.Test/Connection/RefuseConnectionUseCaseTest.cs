using FluentAssertions;
using RecipeBook.Application.UseCases.Connection.RefuseConnection;
using TestsUtilities.Entities;
using TestsUtilities.Hashids;
using TestsUtilities.Repositories;
using TestsUtilities.UserLogged;
using Xunit;

namespace UseCases.Test.Connection;
public class RefuseConnectionUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        var hashids = HashidsBuilder.Instance().Build();

        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Be(hashids.EncodeLong(user.Id));
    }

    private static RefuseConnectionUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
    {
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var writeRepository = CodeWriteOnlyRepositoryBuilder.Instance().Build();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var hashids = HashidsBuilder.Instance().Build();

        return new RefuseConnectionUseCase(writeRepository, userLogged, unitOfWork, hashids);
    }
}
