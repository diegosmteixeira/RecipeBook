using Dapper;
using MySqlConnector;

namespace RecipeBook.Infrastructure.Migrations;

public static class Database
{
    public static void CreateDatabase(string dbConnection, string databaseName)
    {
        using var myConnection = new MySqlConnection(dbConnection);

        var param = new DynamicParameters();
        param.Add("name", databaseName);


        var register = myConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", param);

        if (!register.Any())
        {
            myConnection.Execute($"CREATE DATABASE {databaseName}");
        }
    }

}
