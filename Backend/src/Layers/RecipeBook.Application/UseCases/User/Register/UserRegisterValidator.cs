using FluentValidation;
using RecipeBook.Communication.Request;
using RecipeBook.Exception;
using System.Text.RegularExpressions;

namespace RecipeBook.Application.UseCases.User.Register;

public class UserRegisterValidator : AbstractValidator<RequestUserRegisterJson>
{
    public UserRegisterValidator()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_USERNAME);
        RuleFor(u => u.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_EMAIL);
        RuleFor(u => u.Password).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_PASSWORD);
        RuleFor(u => u.Phone).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_PHONE);
        

        When(u => !string.IsNullOrWhiteSpace(u.Email), () =>
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage(ResourceErrorMessages.INVALID_EMAIL);
        });

        When(u => !string.IsNullOrWhiteSpace(u.Password), () =>
        {
            RuleFor(u => u.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceErrorMessages.PASSWORD_LENGTH);
        });

        When(u => !string.IsNullOrWhiteSpace(u.Phone), () =>
        {
            RuleFor(u => u.Phone).Custom((phone, context) =>
            {
                string phoneStandard = "[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}";
                var isMatch = Regex.IsMatch(phone, phoneStandard);
                if (!isMatch)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(phone),
                        ResourceErrorMessages.INVALID_PHONE));
                }
            });
        });


    }
}
