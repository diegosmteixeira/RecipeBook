using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionNumbers.CreateUserLink, "Adding table for user association")]
public class Version04 : Migration
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        var table = BaseVersion.InsertStandardColumns(Create.Table("Codes"));

        table
            .WithColumn("CodeId").AsString(2000).NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Code_UserId_Id", "Users", "Id");
    }
}
