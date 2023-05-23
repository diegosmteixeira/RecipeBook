using System.Runtime.Serialization;

namespace RecipeBook.Exception.ExceptionsBase;

[Serializable]
public class ValidatorErrorsException : RecipeBookException
{
    public List<string> ErrorMessages { get; set; }
    public ValidatorErrorsException(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;        
    }

    protected ValidatorErrorsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
