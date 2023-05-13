namespace RecipeBook.Exception.ExceptionsBase;

public class ValidatorErrorsException : RecipeBookException
{
    public List<string> ErrorMessages { get; set; }
    public ValidatorErrorsException(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;        
    }
}
