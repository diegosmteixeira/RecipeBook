namespace RecipeBook.Domain.Entities;
public class Recipe : BaseEntity
{
    public string Title { get; set; }
    public Enum.Category Category { get; set; }
    public string Instructions { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
    public long UserId { get; set; }
}