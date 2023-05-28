using FluentValidation;
using RecipeBook.Communication.Request;

namespace RecipeBook.Application.UseCases.Recipe.Register;
public class RecipeRegisterValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeRegisterValidator()
    {
        RuleFor(r => r).SetValidator(new RecipeValidator());
    }
}
