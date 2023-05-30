using FluentAssertions;
using RecipeBook.Application.UseCases.Recipe.Update;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using TestsUtilities.Entities;
using TestsUtilities.Mapper;
using TestsUtilities.Repositories;
using TestsUtilities.Requests;
using TestsUtilities.UserLogged;
using Xunit;

namespace UseCases.Test.Recipe.Update;
public class RecipeUpdateUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        // arrange
        (var user, var _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCaseDependencyInjection(user, recipe);

        var request = RequestRecipeBuilder.Build();

        // act
        await useCase.Execute(recipe.Id, request);

        Func<Task> action = async () => { await useCase.Execute(recipe.Id, request); };

        await action.Should().NotThrowAsync();

        recipe.Title.Should().Be(request.Title);
        recipe.Category.Should().Be((RecipeBook.Domain.Enum.Category)request.Category);
        recipe.Instructions.Should().Be(request.Instructions);
        recipe.Ingredients.Should().HaveCount(request.Ingredients.Count);
    }

    [Fact]
    public async Task Validate_Empty_Ingredient_Failure()
    {
        // arrange
        (var user, var password) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCaseDependencyInjection(user, recipe);

        // act
        var request = RequestRecipeBuilder.Build();
        request.Ingredients.Clear();

        Func<Task> action = async () => { await useCase.Execute(recipe.Id, request); };

        // assert
        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(ex => ex.ErrorMessages.Count == 1 && 
            ex.ErrorMessages.Contains(ResourceErrorMessages.EMPTY_INGREDIENTS));
    }

    [Fact]
    public async Task Validate_Recipe_Not_Exists_Failure()
    {
        // arrange
        (var user, var password) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCaseDependencyInjection(user, recipe);

        // act
        var request = RequestRecipeBuilder.Build();

        Func<Task> action = async () => { await useCase.Execute(0, request); };

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
        var request = RequestRecipeBuilder.Build();
        request.Ingredients.Clear();

        Func<Task> action = async () => { await useCase.Execute(recipe.Id, request); };

        // assert
        await action.Should().ThrowAsync<ValidatorErrorsException>()
            .Where(ex => ex.ErrorMessages.Count == 1 &&
            ex.ErrorMessages.Contains(ResourceErrorMessages.RECIPE_NOTFOUND));
    }

    private static RecipeUpdateUseCase CreateUseCaseDependencyInjection(RecipeBook.Domain.Entities.User user, 
                                                                        RecipeBook.Domain.Entities.Recipe recipe)
    {
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var mapper = MapperBuilder.Instance();
        var repository = RecipeUpdateOnlyRepositoryBuilder.Instance().RecoveryById(recipe).Build();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();

        return new RecipeUpdateUseCase(repository, userLogged, mapper, unitOfWork);

    }
}
