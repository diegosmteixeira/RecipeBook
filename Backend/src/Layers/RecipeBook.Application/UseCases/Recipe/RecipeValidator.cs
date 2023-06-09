﻿using FluentValidation;
using RecipeBook.Communication.Request;
using RecipeBook.Domain.Extension;
using RecipeBook.Exception;

namespace RecipeBook.Application.UseCases.Recipe;
public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(r => r.Title).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_TITLE);
        RuleFor(r => r.Category).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_CATEGORY);
        RuleFor(r => r.Instructions).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_INSTRUCTIONS);
        RuleFor(r => r.PreparationTime).InclusiveBetween(1, 1000);
        RuleFor(r => r.Ingredients).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_INGREDIENTS);

        RuleForEach(r => r.Ingredients).ChildRules(ingredient =>
        {
            ingredient.RuleFor(i => i.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_INGREDIENT_NAME);
            ingredient.RuleFor(i => i.Measurement).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_MEASUREMENT);
        });

        RuleFor(r => r.Ingredients).Custom((ingredients, context) =>
        {
            var distinct = ingredients.Select(i => i.Name.AccentRemover().ToLower()).Distinct();
            if (distinct.Count() != ingredients.Count())
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredients", ResourceErrorMessages.REPEATED_INGREDIENTS));
            }
        });
    }
}
