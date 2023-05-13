using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeBook.Communication.Response;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using System.Net;

namespace RecipeBook.API.Filters;

public class ExceptionsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is RecipeBookException)
        {
            PersonalizeException(context);
        }
        else
        {
            ThrowUnknowException(context);
        }
    }

    private void PersonalizeException(ExceptionContext context)
    {
        if (context.Exception is ValidatorErrorsException)
        {
            ExceptionHandler(context);
        }
    }
    private void ExceptionHandler(ExceptionContext context)
    {
        var validatorErrors = context.Exception as ValidatorErrorsException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ErrorResponseJson(validatorErrors.ErrorMessages));
    }

    private void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ErrorResponseJson(ResourceErrorMessages.UNKNOWN_ERROR));
    }


}
