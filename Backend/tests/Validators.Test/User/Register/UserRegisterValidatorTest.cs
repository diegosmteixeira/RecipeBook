using FluentAssertions;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Exception;
using TestsUtilities.Requests;
using Xunit;

namespace Validators.Test.User.Register;

public class UserRegisterValidatorTest
{
    [Fact]
    public void Validate_Success()
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Empty_Name_Error()
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => 
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_USERNAME));
    }

    [Fact]
    public void Validate_Empty_Password_Error()
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build();
        request.Password = string.Empty;

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
    public void Validate_Password_Length_Error(int passwordLength)
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build(passwordLength);
        request.Password = "1234";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_LENGTH));
    }

    [Fact]
    public void Validate_Empty_Email_Error()
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_EMAIL));
    }



    [Fact]
    public void Validate_Invalid_Email_Error()
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build();
        request.Email = "email";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_EMAIL));
    }

    [Fact]
    public void Validate_Email_Already_Exists_Error()
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build();
        request.Email = "email";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_EMAIL));
    }

    [Fact]
    public void Validate_Empty_Phone_Error()
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build();
        request.Phone = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_PHONE));
    }

    [Fact]
    public void Validate_Standard_Phone_Error()
    {
        var validator = new UserRegisterValidator();

        var request = RequestUserRegisterBuilder.Build();
        request.Phone = "123 21 333-333";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PHONE));
    }
}
