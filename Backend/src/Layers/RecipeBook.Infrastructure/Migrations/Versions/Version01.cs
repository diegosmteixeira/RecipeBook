using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionNumbers.CreateUserTable, "Create user table")]
public class Version01 : Migration
{
    public override void Down()
    {
    }

    public override void Up()
    {
        var table = BaseVersion.InsertStandardColumns(Create.Table("User"));

        table
             .WithColumn("Name").AsString(100).NotNullable()
             .WithColumn("Email").AsString().NotNullable()
             .WithColumn("Password").AsString(2000).NotNullable()
             .WithColumn("Phone").AsString(14).NotNullable();
    }
}
