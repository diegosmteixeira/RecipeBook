namespace RecipeBook.Application.Services.LoggedUser;

public interface IUserLogged
{
    Task<Domain.Entities.User> UserRecovery();
}
