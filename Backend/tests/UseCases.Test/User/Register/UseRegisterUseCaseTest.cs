using Xunit;
using TestsUtilities.Requests;
using RecipeBook.Application.UseCases.User.Register;
using TestsUtilities.Repositories;
using TestsUtilities.Mapper;
using TestsUtilities.Cryptography;
using TestsUtilities.Token;
using FluentAssertions;
using RecipeBook.Exception.ExceptionsBase;
using RecipeBook.Exception;

namespace UseCases.Test.User.Register;

public class UseRegisterUseCaseTest
{
    [Fact]
    public async Task Validate_Sucess()
    {
        var request = RequestUserRegisterBuilder.Build();

        var useCase = CreateUseCase();

        var response = await useCase.Execute(request);

        response.Should().NotBeNull();
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Email_AlreadyExists_Error()
    {
        var request = RequestUserRegisterBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> action = async () => { await useCase.Execute(request); };

        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(exception => 
                exception.ErrorMessages.Count == 1 && 
                exception.ErrorMessages.Contains(ResourceErrorMessages.EMAIL_EXISTS));
    }

    [Fact]
    public async Task Validate_User_Empty_Field_Error()
    {
        var request = RequestUserRegisterBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> action = async () => { await useCase.Execute(request); };

        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(exception =>
                exception.ErrorMessages.Count == 1 &&
                exception.ErrorMessages.Contains(ResourceErrorMessages.EMPTY_EMAIL));
    }

    private static UserRegisterUseCase CreateUseCase(string email = "")
    {
        var mapper = MapperBuilder.Instance();
        var repository = UserWriteOnlyRepositoryBuilder.Instance().Build();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var encryption = EncryptionBuilder.Instance();
        var token = TokenConfiguratorBuilder.Instance();
        var readOnly = UserReadOnlyRepositoryBuilder.Instance().CheckIfUserExists(email).Build();

        return new UserRegisterUseCase(repository, mapper, unitOfWork, encryption, token, readOnly);

    }
}
