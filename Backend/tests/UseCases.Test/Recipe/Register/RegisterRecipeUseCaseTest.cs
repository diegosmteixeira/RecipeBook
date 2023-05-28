using AutoMapper;
using FluentAssertions;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using TestsUtilities.Entities;
using TestsUtilities.Mapper;
using TestsUtilities.Repositories;
using TestsUtilities.Requests;
using TestsUtilities.UserLogged;
using Xunit;

namespace UseCases.Test.Recipe.Register;
public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        // arrange
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = RequestRecipeRegisterBuilder.Build();
        
        // act
        var response = await useCase.Execute(request);

        // asserts
        response.Id.Should().NotBeNullOrWhiteSpace();
        response.Title.Should().Be(request.Title);
        response.Category.Should().Be(request.Category);
        response.Instructions.Should().Be(request.Instructions);
    }

    [Fact]
    public async Task Validate_Empty_Ingredient_Failure()
    {
        // arrange
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = RequestRecipeRegisterBuilder.Build();
        request.Ingredients.Clear();

        // act
        Func<Task> action = async () => { await useCase.Execute(request); };

        // asserts
        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(exception => exception.ErrorMessages.Count == 1 &&
                exception.ErrorMessages.Contains(ResourceErrorMessages.EMPTY_INGREDIENTS));
    }

    private static RecipeRegisterUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Instance();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var repository = RecipeWriteOnlyRepositoryBuilder.Instance().Build();

        return new RecipeRegisterUseCase(mapper, unitOfWork, userLogged,  repository);
    }
}
