using Bogus;
using RecipeBook.Communication.Response;
using TestsUtilities.Hashids;

namespace TestsUtilities.Responses;
public class ResponseUserConnectionJsonBuilder
{
    public static ResponseUserConnectionJson Build()
    {
        var hashId = HashidsBuilder.Instance().Build();

        return new Faker<ResponseUserConnectionJson>()
            .RuleFor(r => r.Id, f => hashId.EncodeLong(f.Random.Long(1, 5000)))
            .RuleFor(r => r.Name, f => f.Person.FullName);
    }
}
