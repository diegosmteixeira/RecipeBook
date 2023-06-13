using FluentAssertions;
using RecipeBook.Application.UseCases.Connection.RecoverConnection;
using TestsUtilities.Entities;
using TestsUtilities.Mapper;
using TestsUtilities.Repositories;
using TestsUtilities.UserLogged;
using Xunit;

namespace UseCases.Test.Connection;
public class RecoverConnectionUseCaseTest
{
    [Theory]
    [InlineData(7)]
    [InlineData(4)]
    [InlineData(3)]
    [InlineData(9)]
    public async Task Validate_Success(int recipeQuantity)
    {
        (var user, var _) = UserBuilder.Build();

<<<<<<< HEAD
        var connections = TestsUtilities.Entities.ConnectionBuilder.Build();
=======
        var connections = ConnectionBuilder.Build();
>>>>>>> feature/recipe-recovery-by-id

        var useCase = CreateUseCase(connections, user, recipeQuantity);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Users.Should().NotBeEmpty();
        result.Users.Should().HaveCount(connections.Count);
        result.Users.Should().SatisfyRespectively
        (
            firstElement =>
            {
                firstElement.Id.Should().NotBeNullOrWhiteSpace();
                firstElement.Name.Should().Be(connections.First().Name);
                firstElement.RecipesQuantity.Should().Be(recipeQuantity);
            }
        );
    }

    private static RecoverConnectionUseCase CreateUseCase(IList<RecipeBook.Domain.Entities.User> connections,
                                                          RecipeBook.Domain.Entities.User user,
                                                          int recipeQuantity)
    {
        var userLogged = UserLoggedBuilder.Instance().UserRecovery(user).Build();
        var connRepository = ConnectionReadOnlyRepositoryBuilder.Instance().RecoverConnections(user, connections).Build();
        var recipeRepository = RecipeReadOnlyRepositoryBuilder.Instance().RecipeRecoveryCount(recipeQuantity).Build();
        var autoMapper = MapperBuilder.Instance();

        return new RecoverConnectionUseCase(userLogged, connRepository, recipeRepository, autoMapper);
    }
}
