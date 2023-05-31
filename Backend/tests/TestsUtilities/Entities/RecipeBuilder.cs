using Bogus;
using RecipeBook.Domain.Entities;

namespace TestsUtilities.Entities;
public class RecipeBuilder
{
    public static Recipe Build(User user)
    {
        return new Faker<Recipe>()
            .RuleFor(r => r.Id, _ => user.Id)
            .RuleFor(r => r.Title, f => f.Commerce.Department())
            .RuleFor(r => r.Category, f => f.PickRandom<RecipeBook.Domain.Enum.Category>())
            .RuleFor(r => r.Instructions, f => f.Commerce.ProductDescription())
            .RuleFor(r => r.PreparationTime, f => f.Random.Int(1, 1000))
            .RuleFor(r => r.Ingredients, f => RandomIngredients(f, user.Id))
            .RuleFor(r => r.UserId, _ => user.Id);
    }

    private static List<Ingredient> RandomIngredients(Faker f, long  id)
    {
        List<Ingredient> ingredients = new List<Ingredient>();

        for (int i = 0; i < f.Random.Int(1, 5); i++)
        {
            ingredients.Add(new Ingredient()
            {
                Id = (id * 100) + (i + 1),
                Name = f.Commerce.ProductName(),
                Measurement = $"{f.Random.Double(1, 10)} {f.Random.Word()}"
            });
        }

        return ingredients;
    }
}
