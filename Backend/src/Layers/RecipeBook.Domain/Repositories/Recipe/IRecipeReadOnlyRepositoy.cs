﻿namespace RecipeBook.Domain.Repositories.Recipe;
public interface IRecipeReadOnlyRepositoy
{
    Task<IList<Entities.Recipe>> RecipeRecovery(long userId);
}
