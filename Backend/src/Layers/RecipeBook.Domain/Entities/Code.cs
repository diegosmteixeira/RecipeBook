namespace RecipeBook.Domain.Entities;
public class Code : BaseEntity
{
    public string CodeId { get; set; }
    public long UserId { get; set; }
}
