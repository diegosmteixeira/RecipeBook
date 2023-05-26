using Bogus;
using RecipeBook.Communication.Enum;
using RecipeBook.Communication.Request;

namespace TestsUtilities.Requests;
public class RequestRecipeRegisterBuilder
{
    public static RequestRecipeRegisterJson Build()
    {
        return new Faker<RequestRecipeRegisterJson>()
            .RuleFor(r => r.Title, f => f.Lorem.Word())
            .RuleFor(r => r.Category, f => f.PickRandom<Category>())
            .RuleFor(r => r.Instructions, f => f.Lorem.Paragraph())
            .RuleFor(r => r.Ingredients, f => Ingredients(f));
    }

    private static List<RequestIngredientRegisterJson> Ingredients(Faker faker)
    {
        List<RequestIngredientRegisterJson> ingredients = new();

        for (int i = 0; i < faker.Random.Int(1, 5); i++)
        {
            ingredients.Add(new RequestIngredientRegisterJson()
            {
                Name = faker.Commerce.ProductName(),
                Measurement = $"{faker.Random.Double(1, 10)} {faker.Random.Word()}"
            });
        }
        return ingredients;
    }
}