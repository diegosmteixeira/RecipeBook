using FluentAssertions;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Exception;
using TestsUtilities.Requests;
using Xunit;

namespace Validators.Test.User.ChangePassword;


public class ChangePasswordValidatorTest
{
    [Fact]
    public void Validate_Success()
    {
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordUserBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Empty_Password_Error()
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordUserBuilder.Build();
        request.NewPassword = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_PASSWORD));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_Password_Length_Error(int password)
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordUserBuilder.Build(password);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_LENGTH));
    }
}
