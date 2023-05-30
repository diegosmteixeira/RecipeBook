using Bogus;
using RecipeBook.Communication.Enum;
using RecipeBook.Communication.Request;

namespace TestsUtilities.Requests;
public class RequestRecipeBuilder
{
    public static RequestRecipeJson Build()
    {
        return new Faker<RequestRecipeJson>()
            .RuleFor(r => r.Title, f => f.Lorem.Word())
            .RuleFor(r => r.Category, f => f.PickRandom<Category>())
            .RuleFor(r => r.Instructions, f => f.Lorem.Paragraph())
            .RuleFor(r => r.Ingredients, f => Ingredients(f));
    }

    private static List<RequestIngredientJson> Ingredients(Faker faker)
    {
        List<RequestIngredientJson> ingredients = new();

        for (int i = 0; i < faker.Random.Int(1, 5); i++)
        {
            ingredients.Add(new RequestIngredientJson()
            {
                Name = faker.Commerce.ProductName(),
                Measurement = $"{faker.Random.Double(1, 10)} {faker.Random.Word()}"
            });
        }
        return ingredients;
    }
}