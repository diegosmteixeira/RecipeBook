using FluentAssertions;
using RecipeBook.Exception.ExceptionsBase;
using RecipeBook.Exception;
using TestsUtilities.Entities;
using TestsUtilities.Mapper;
using TestsUtilities.Repositories;
using TestsUtilities.UserLogged;
using Xunit;
using RecipeBook.Application.UseCases.Recipe.Recover;

namespace UseCases.Test.Recipe.RecoverById;
public class RecipeRecoverByIdUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        // arrange
        (var user, var _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCaseDependencyInjection(user, recipe);

        // act
        var response = await useCase.Execute(recipe.Id);

        response.Title.Should().Be(recipe.Title);
        response.Category.Should().Be((RecipeBook.Communication.Enum.Category)recipe.Category);
        response.Instructions.Should().Be(recipe.Instructions);
        response.Ingredients.Should().HaveCount(recipe.Ingredients.Count);
    }

    [Fact]
    public async Task Validate_Recipe_Not_Exists_Failure()
    {
        // arrange
        (var user, var password) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCaseDependencyInjection(user, recipe);

        // act
        Func<Task> action = async () => { await useCase.Execute(0); };

        // assert
        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(ex => ex.ErrorMessages.Count == 1 &&
            ex.ErrorMessages.Contains(ResourceErrorMessages.RECIPE_NOTFOUND));
    }

    [Fact]
    public async Task Validate_UserLogged_Recipe_Owner_Failure()
    {
        // arrange
        (var user, var password) = UserBuilder.Build();
        (var user77, _) = UserBuilder.BuildUser2();

        var recipe = RecipeBuilder.Build(user77);

        var useCase = CreateUseCaseDependencyInjection(user, recipe);

        // act

        Func<Task> action = async () => { await useCase.Execute(recipe.Id); };

        // assert
        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(ex => ex.ErrorMessages.Count == 1 &&
            ex.ErrorMessages.Contains(ResourceErrorMessages.RECIPE_NOTFOUND));
    }

    private static RecipeRecoveryByIdUseCase CreateUseCaseDependencyInjection(RecipeBook.Domain.Entities.User user,
                                                                              RecipeBook.Domain.Entities.Recipe recipe)
    {
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var mapper = MapperBuilder.Instance();
        var repository = RecipeReadOnlyRepositoryBuilder.Instance().RecoverById(recipe).Build();

        return new RecipeRecoveryByIdUseCase(repository, userLogged, mapper);
    }
}
