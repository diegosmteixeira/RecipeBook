using FluentValidation;
using RecipeBook.Communication.Request;

namespace RecipeBook.Application.UseCases.Recipe.Register;
public class RegisterRecipeValidator : AbstractValidator<RequestRecipeRegisterJson>
{
    public RegisterRecipeValidator()
    {
        RuleFor(r => r.Title).NotEmpty();
        RuleFor(r => r.Category).IsInEnum();
        RuleFor(r => r.Instructions).NotEmpty();
        RuleFor(r => r.Ingredients).NotEmpty();

        RuleForEach(r => r.Ingredients).ChildRules(ingredient =>
        {
            ingredient.RuleFor(i => i.Name).NotEmpty();
            ingredient.RuleFor(i => i.Measurement).NotEmpty();
        });

        RuleFor(r => r.Ingredients).Custom((ingredients, context) =>
        {
            var distinct = ingredients.Select(i => i.Name).Distinct();
            if (distinct.Count() != ingredients.Count())
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredients", ""));
            }
        });
    }
}
