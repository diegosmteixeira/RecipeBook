using FluentValidation;
using RecipeBook.Exception;

namespace RecipeBook.Application.UseCases.User
{
    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(stringPassword => stringPassword).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_PASSWORD);

            When(stringPassword => !string.IsNullOrWhiteSpace(stringPassword), () =>
            {
                RuleFor(stringPassword => stringPassword.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceErrorMessages.PASSWORD_LENGTH);
            });
        }
    }
}
