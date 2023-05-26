using FluentValidation;
using RecipeBook.Communication.Request;
using RecipeBook.Exception;

namespace RecipeBook.Application.UseCases.Recipe.Register;
public class RegisterRecipeValidator : AbstractValidator<RequestRecipeRegisterJson>
{
    public RegisterRecipeValidator()
    {
        RuleFor(r => r.Title).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_TITLE);
        RuleFor(r => r.Category).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_CATEGORY);
        RuleFor(r => r.Instructions).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_INSTRUCTIONS);
        RuleFor(r => r.Ingredients).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_INGREDIENTS);

        RuleForEach(r => r.Ingredients).ChildRules(ingredient =>
        {
            ingredient.RuleFor(i => i.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_INGREDIENT_NAME);
            ingredient.RuleFor(i => i.Measurement).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_MEASUREMENT);
        });

        RuleFor(r => r.Ingredients).Custom((ingredients, context) =>
        {
            var distinct = ingredients.Select(i => i.Name).Distinct();
            if (distinct.Count() != ingredients.Count())
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredients", ResourceErrorMessages.REPEATED_INGREDIENTS));
            }
        });
    }
}
