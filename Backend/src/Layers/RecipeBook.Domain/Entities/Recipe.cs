using RecipeBook.Domain.Enum;

namespace RecipeBook.Domain.Entities;
public class Recipe : BaseEntity
{
    public string Title { get; set; }
    public Category Categories { get; set; }
    public string Instructions { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
}