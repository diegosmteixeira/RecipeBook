namespace RecipeBook.Communication.Request;

public class RequestChangePasswordJson
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
