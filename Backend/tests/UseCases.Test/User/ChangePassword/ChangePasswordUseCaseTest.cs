using FluentAssertions;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Communication.Request;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using TestsUtilities.Cryptography;
using TestsUtilities.Entities;
using TestsUtilities.Repositories;
using TestsUtilities.Requests;
using TestsUtilities.UserLogged;
using Xunit;

namespace WebApi.Test.V1.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = RequestChangePasswordUserBuilder.Build();
        request.CurrentPassword = password;

        Func<Task> action = async () =>
        {
            await useCase.Execute(new RequestChangePasswordJson
            {
                CurrentPassword = password,
                NewPassword = "@NewPassword1234"
            });
        };

        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Validate_Empty_NewPassword_Error()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(new RequestChangePasswordJson
            {
                CurrentPassword = password,
                NewPassword = ""
            });
        };

        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(e => e.ErrorMessages.Count == 1 && 
                        e.ErrorMessages.Contains(ResourceErrorMessages.EMPTY_PASSWORD));
    }

    [Fact]
    public async Task Validate_Invalid_CurrentPassword_Error()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = RequestChangePasswordUserBuilder.Build();
        request.CurrentPassword = "invalid-Password";

        Func<Task> action = async () =>
        {
            await useCase.Execute(request);
        };

        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(e => e.ErrorMessages.Count == 1 &&
                        e.ErrorMessages.Contains(ResourceErrorMessages.INVALID_CURRENT_PASSWORD));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validate_MinLength_CurrentPassword_Error(int passwordLength)
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = RequestChangePasswordUserBuilder.Build(passwordLength);
        request.CurrentPassword = password;

        Func<Task> action = async () =>
        {
            await useCase.Execute(request);
        };

        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(e => e.ErrorMessages.Count == 1 &&
                        e.ErrorMessages.Contains(ResourceErrorMessages.PASSWORD_LENGTH));
    }

    private static ChangePasswordUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
    {
        var encryption = EncryptionBuilder.Instance();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var repository = UserUpdateOnlyRepositoryBuilder.Instance().UserRecoveryById(user).Build();
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();

        return new ChangePasswordUseCase(repository, userLogged, encryption, unitOfWork);
    }
}
