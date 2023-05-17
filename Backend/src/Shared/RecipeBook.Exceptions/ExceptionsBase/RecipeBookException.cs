namespace RecipeBook.Exception.ExceptionsBase;

public class RecipeBookException : SystemException
{
    public RecipeBookException()
    {
    }

    public RecipeBookException(string message) : base(message)
    {        
    }
}
