using FluentValidation;
using RecipeBook.Communication.Request;

namespace RecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(request => request.NewPassword).SetValidator(new PasswordValidator());   
    }
}
