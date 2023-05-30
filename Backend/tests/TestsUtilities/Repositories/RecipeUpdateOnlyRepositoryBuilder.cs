using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsUtilities.Repositories;
public class RecipeUpdateOnlyRepositoryBuilder
{
    private static RecipeUpdateOnlyRepositoryBuilder _instance;
    private readonly Mock<IRecipeUpdateOnlyRepository> _repository;
    private RecipeUpdateOnlyRepositoryBuilder()
    {
        if (_repository is null)
        {
            _repository = new Mock<IRecipeUpdateOnlyRepository>();
        }
    }

    public static RecipeUpdateOnlyRepositoryBuilder Instance()
    {
        _instance = new RecipeUpdateOnlyRepositoryBuilder();
        return _instance;
    }

    public IRecipeUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }

    public RecipeUpdateOnlyRepositoryBuilder RecoveryById(Recipe recipe)
    {
        _repository.Setup(repo => repo.RecipeRecoveryById(recipe.Id)).ReturnsAsync(recipe);

        return this;
    }
}
