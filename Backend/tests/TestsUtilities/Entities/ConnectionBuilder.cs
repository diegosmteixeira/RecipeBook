using Bogus;
using RecipeBook.Domain.Entities;
using TestsUtilities.Cryptography;

namespace TestsUtilities.Entities;
public class ConnectionBuilder
{
    public static List<User> Build()
    {
        var userConnection = CreateUser();
        userConnection.Id = 2;

        return new List<User>() { userConnection };
    }

    public static User CreateUser()
    {
        var userCreated = new Faker<User>()
            .RuleFor(u => u.Id, f => 1)
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f =>
            {
                var password = f.Internet.Password();

                return EncryptionBuilder.Instance().PasswordEncryption(password);
            })
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("## ! ####-####")
                .Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));

        return userCreated;
    }
}
