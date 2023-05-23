using Bogus;
using RecipeBook.Communication.Request;

namespace TestsUtilities.Requests;

public class RequestChangePasswordUserBuilder
{
    public static RequestChangePasswordJson Build(int password = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(u => u.CurrentPassword, f => f.Internet.Password(10))
            .RuleFor(u => u.NewPassword, f => f.Internet.Password(password));
    }
}