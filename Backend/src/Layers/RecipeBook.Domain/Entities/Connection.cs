namespace RecipeBook.Domain.Entities;
public class Connection : BaseEntity
{
    public long UserId { get; set; }
    public long ConnectedWithUserId { get; set; }
    public User ConnectedWithUser { get; set; }
}
