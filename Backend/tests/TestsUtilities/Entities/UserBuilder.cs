using Bogus;
using RecipeBook.Domain.Entities;
using TestsUtilities.Cryptography;

namespace TestsUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        string password = string.Empty;

        var userCreated = new Faker<User>()
            .RuleFor(u => u.Id, f => 1)
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => 
            {
                password = f.Internet.Password();

                return EncryptionBuilder.Instance().PasswordEncryption(password);
            })
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("## ! ####-####")
                .Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));

        return (userCreated, password);
    }

    public static (User user, string password) BuildUser2()
    {
        (var user, var password) = Build();
        user.Id = 3;

        return (user, password);
    }
}
