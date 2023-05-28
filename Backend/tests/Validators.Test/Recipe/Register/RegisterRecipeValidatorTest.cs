using FluentAssertions;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Communication.Enum;
using RecipeBook.Exception;
using TestsUtilities.Requests;
using Xunit;

namespace Validators.Test.Recipe.Register;
public class RegisterRecipeValidatorTest
{

    [Fact]
    public void Validate_Success()
    {
        // arrange
        var validator = new RecipeRegisterValidator();
        var request = RequestRecipeRegisterBuilder.Build();

        // act
        var result = validator.Validate(request);

        // FluentAssertions
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Empty_Title_Failure()
    {
        // arrange
        var validator = new RecipeRegisterValidator();
        var request = RequestRecipeRegisterBuilder.Build();
        request.Title = "";

        // act
        var result = validator.Validate(request);

        // assert

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_TITLE));
    }

    [Fact]
    public void Validate_Invalid_Category_Failure()
    {
        // arrange
        var validator = new RecipeRegisterValidator();
        var request = RequestRecipeRegisterBuilder.Build();
        request.Category = (Category)10;

        // act
        var result = validator.Validate(request);

        // assert

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_CATEGORY));

    }

    [Fact]
    public void Validate_Empty_Instructions_Failure()
    {
        // arrange
        var validator = new RecipeRegisterValidator();
        var request = RequestRecipeRegisterBuilder.Build();
        request.Instructions = string.Empty;

        // act
        var result = validator.Validate(request);

        // assert

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_INSTRUCTIONS));
    }

    [Fact]
    public void Validate_Empty_Ingredients_Failure()
    {
        // arrange
        var validator = new RecipeRegisterValidator();
        var request = RequestRecipeRegisterBuilder.Build();
        request.Ingredients.Clear();

        // act
        var result = validator.Validate(request);

        // assert

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_INGREDIENTS));
    }

    [Fact]
    public void Validate_Empty_Ingredient_Name_Failure()
    {
        // arrange
        var validator = new RecipeRegisterValidator();
        var request = RequestRecipeRegisterBuilder.Build();
        request.Ingredients.First().Name = string.Empty;

        // act
        var result = validator.Validate(request);

        // assert

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_INGREDIENT_NAME));
    }

    [Fact]
    public void Validate_Empty_Ingredient_Measurement_Failure()
    {
        // arrange
        var validator = new RecipeRegisterValidator();
        var request = RequestRecipeRegisterBuilder.Build();
        request.Ingredients.First().Measurement = string.Empty;

        // act
        var result = validator.Validate(request);

        // assert

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_MEASUREMENT));
    }

    [Fact]
    public void Validate_Repeated_Ingredient_Failure()
    {
        // arrange
        var validator = new RecipeRegisterValidator();
        var request = RequestRecipeRegisterBuilder.Build();
        request.Ingredients.Add(request.Ingredients.First());

        // act
        var result = validator.Validate(request);

        // assert

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.REPEATED_INGREDIENTS));
    }
}
