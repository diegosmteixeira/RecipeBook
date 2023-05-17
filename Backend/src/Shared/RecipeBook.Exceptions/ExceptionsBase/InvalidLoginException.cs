namespace RecipeBook.Exception.ExceptionsBase;

public class InvalidLoginException : RecipeBookException
{
    public InvalidLoginException() : base(ResourceErrorMessages.INVALID_LOGIN)
    {
    }
}
