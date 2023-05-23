using System.Runtime.Serialization;

namespace RecipeBook.Exception.ExceptionsBase;

[Serializable]
public class InvalidLoginException : RecipeBookException
{
    public InvalidLoginException() : base(ResourceErrorMessages.INVALID_LOGIN)
    {
    }

    protected InvalidLoginException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}