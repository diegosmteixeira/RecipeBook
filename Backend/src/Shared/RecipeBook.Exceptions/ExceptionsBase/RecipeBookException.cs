using System.Runtime.Serialization;

namespace RecipeBook.Exception.ExceptionsBase;

[Serializable]
public class RecipeBookException : SystemException
{
    public RecipeBookException()
    {
    }

    public RecipeBookException(string message) : base(message)
    {
    }

    protected RecipeBookException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
