using System.Collections;
using TestsUtilities.Entities;

namespace UseCases.Test.Connection.InlineData;
public class UserEntitiesConnectionDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var connections = ConnectionBuilder.Build();

        return connections.Select(connection => new object[] { connection.Id, connections }).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
