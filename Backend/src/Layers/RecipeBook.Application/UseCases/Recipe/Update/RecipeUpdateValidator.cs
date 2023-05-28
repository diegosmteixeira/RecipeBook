using FluentValidation;
using RecipeBook.Communication.Request;
using RecipeBook.Exception;

namespace RecipeBook.Application.UseCases.Recipe.Update;
public class RecipeUpdateValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeUpdateValidator()
    {
        RuleFor(r => r).SetValidator(new RecipeValidator());
    }
}
