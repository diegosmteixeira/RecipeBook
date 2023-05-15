using Bogus;
using RecipeBook.Communication.Request;

namespace TestsUtilities.Requests;

public class RequestUserRegisterBuilder
{
    public static RequestUserRegisterJson Build(int passwordLength = 10)
    {
        return new Faker<RequestUserRegisterJson>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password(passwordLength))
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("## ! ####-####")
                .Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));
    }
}
