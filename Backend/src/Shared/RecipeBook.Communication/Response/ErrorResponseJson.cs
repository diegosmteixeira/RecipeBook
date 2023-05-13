namespace RecipeBook.Communication.Response;

public class ErrorResponseJson
{
    public List<string> Errors { get; set; }

    public ErrorResponseJson(string error)
    {
        Errors = new List<string> { error };
    }

    public ErrorResponseJson(List<string> errors)
    {
        Errors = errors;
    }
}
