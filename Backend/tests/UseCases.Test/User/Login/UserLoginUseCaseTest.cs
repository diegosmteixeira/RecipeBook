using FluentAssertions;
using RecipeBook.Application.UseCases.User.Login;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using TestsUtilities.Cryptography;
using TestsUtilities.Repositories;
using TestsUtilities.Token;
using Xunit;

namespace UseCases.Test.User.Login;

public class UserLoginUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        (var user, var password) = TestsUtilities.Entities.UserBuilder.Build();

        var useCase = CreateUserForTest(user);

        var response = await useCase.Execute(new RecipeBook.Communication.Request.RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Invalid_Password_Error()
    {
        (var user, var password) = TestsUtilities.Entities.UserBuilder.Build();

        var useCase = CreateUserForTest(user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(new RecipeBook.Communication.Request.RequestLoginJson
            {
                Email = user.Email,
                Password = "invalid-Password"
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }

    [Fact]
    public async Task Validate_Invalid_Email_Error()
    {
        (var user, var password) = TestsUtilities.Entities.UserBuilder.Build();

        var useCase = CreateUserForTest(user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(new RecipeBook.Communication.Request.RequestLoginJson
            {
                Email = "invalid-Email",
                Password = password
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }

    [Fact]
    public async Task Validate_Invalid_Email_And_Password_Error()
    {
        (var user, var password) = TestsUtilities.Entities.UserBuilder.Build();

        var useCase = CreateUserForTest(user);

        Func<Task> action = async () =>
        {
            await useCase.Execute(new RecipeBook.Communication.Request.RequestLoginJson
            {
                Email = "invalid-Email",
                Password = "invalid-Passowrd"
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
           .Where(exception => exception.Message.Equals(ResourceErrorMessages.INVALID_LOGIN));
    }


    private UserLoginUseCase CreateUserForTest(RecipeBook.Domain.Entities.User user)
    {
        var repository = UserReadOnlyRepositoryBuilder.Instance().Login(user).Build();
        var token = TokenConfiguratorBuilder.Instance();
        var encryption = EncryptionBuilder.Instance();

        return new UserLoginUseCase(repository, token, encryption);
    }
}
