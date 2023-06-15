using FluentAssertions;
using RecipeBook.Application.UseCases.Connection.ReadQRCode;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using System.Reflection;
using TestsUtilities.Entities;
using TestsUtilities.Hashids;
using TestsUtilities.Repositories;
using TestsUtilities.UserLogged;
using Xunit;

namespace UseCases.Test.Connection;
public class ReadQRCodeUseCaseTest
{
    [Fact]
    public async Task Vaidate_Success()
    {
        (var userToConnect, var _) = UserBuilder.Build();
        (var idUserWhichGenerateQRCode, var _) = UserBuilder.BuildUser2();

        var code =  CodeBuilder.Build(idUserWhichGenerateQRCode);

        var useCase = CreateUseCase(userToConnect, code);

        var result = await useCase.Execute(code.CodeId);

        result.Should().NotBeNull();
        result.userToConnect.Should().NotBeNull();

        var hashids = HashidsBuilder.Instance().Build();
        result.idUserWhichGenerateQRCode.Should().Be(hashids.EncodeLong(idUserWhichGenerateQRCode.Id));
    }

    [Fact]
    public async Task Validate_Code_Not_Found_Failure()
    {
        (var userToConnect, var _) = UserBuilder.Build();
        (var idUserWhichGenerateQRCode, var _) = UserBuilder.BuildUser2();

        var code = CodeBuilder.Build(idUserWhichGenerateQRCode);

        var useCase = CreateUseCase(userToConnect, code);

        Func<Task> action = async () =>
        {
            await useCase.Execute(Guid.NewGuid().ToString());
        };

        await action.Should().ThrowAsync<RecipeBookException>()
            .Where(ex => ex.Message.Equals(ResourceErrorMessages.CODE_NOT_FOUND));
    }

    [Fact]
    public async Task Validate_Connection_Already_Exists_Failure()
    {
        (var userToConnect, var _) = UserBuilder.Build();
        (var idUserWhichGenerateQRCode, var _) = UserBuilder.BuildUser2();

        var code = CodeBuilder.Build(idUserWhichGenerateQRCode);

        var useCase = CreateUseCase(userToConnect, 
                                    code, 
                                    idUserWhichGenerateQRCode.Id, 
                                    userToConnect.Id);

        Func<Task> action = async () =>
        {
            await useCase.Execute(code.CodeId);
        };

        await action.Should().ThrowAsync<RecipeBookException>()
            .Where(ex => ex.Message.Equals(ResourceErrorMessages.CONN_EXISTS));
    }

    private static ReadQRCodeUseCase CreateUseCase(RecipeBook.Domain.Entities.User user,
                                                   RecipeBook.Domain.Entities.Code code,
                                                   long? userToConnect = null, 
                                                   long? idUserWhichGenerateQRCode = null) 
    {
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var hashids = HashidsBuilder.Instance().Build();
        var codeRepository = CodeReadOnlyRepositoryBuilder.Instance().RecoverEntityCode(code).Build();
        var connectionRepository = ConnectionReadOnlyRepositoryBuilder.Instance()
            .IsConnectedAsync(userToConnect, idUserWhichGenerateQRCode).Build();

        return new ReadQRCodeUseCase(connectionRepository, codeRepository, userLogged, hashids);
    }
}
