﻿namespace RecipeBook.Domain.Repositories.Recipe;
public interface IRecipeWriteOnlyRepository
{
    Task AddRecipe(Entities.Recipe recipe);
}
