using FluentAssertions;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;
using TestsUtilities.Entities;
using TestsUtilities.Hashids;
using TestsUtilities.Repositories;
using TestsUtilities.UserLogged;
using Xunit;

namespace UseCases.Test.Connection;
public class GenerateQRCodeUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.qrCode.Should().NotBeEmpty();

        var hashids = HashidsBuilder.Instance().Build();
        result.idUser.Should().Be(hashids.EncodeLong(user.Id));
    }

    private static GenerateQRCodeUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
    {
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var writeRepository = CodeWriteOnlyRepositoryBuilder.Instance().Build();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var hashids = HashidsBuilder.Instance().Build();

        return new GenerateQRCodeUseCase(writeRepository, userLogged, unitOfWork, hashids);
    }

}
