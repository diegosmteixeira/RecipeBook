using RecipeBook.Communication.Enum;

namespace RecipeBook.Communication.Request;
public class RequestDashboardJson
{
    public string TitleOrIngredient { get; set; }
    public Category? Category { get; set; }

}
