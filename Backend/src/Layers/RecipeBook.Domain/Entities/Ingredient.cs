namespace RecipeBook.Domain.Entities;
public class Ingredient : BaseEntity
{
    public string Name { get; set; }
    public string Measurement { get; set; }
    public long RecipeId { get; set; }
}