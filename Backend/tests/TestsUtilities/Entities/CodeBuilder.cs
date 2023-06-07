using Bogus;
using RecipeBook.Domain.Entities;
using System;

namespace TestsUtilities.Entities;
public class CodeBuilder
{
    public static Code Build(User user)
    {
        return new Faker<Code>()
            .RuleFor(c => c.Id, _ => user.Id)
            .RuleFor(c => c.UserId, _ => user.Id)
            .RuleFor(c => c.CodeId, _ => Guid.NewGuid().ToString());
    }
}
