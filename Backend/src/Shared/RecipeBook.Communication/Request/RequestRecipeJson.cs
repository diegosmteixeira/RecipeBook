using RecipeBook.Communication.Enum;
namespace RecipeBook.Communication.Request;
public class RequestRecipeJson
{
    public RequestRecipeJson()
    {
        Ingredients = new();
    }

    public string Title { get; set; }
    public Category Category { get; set; }
    public string Instructions { get; set; }
    public List<RequestIngredientJson> Ingredients { get; set; }
}
