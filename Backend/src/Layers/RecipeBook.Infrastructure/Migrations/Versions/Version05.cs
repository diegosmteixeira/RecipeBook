using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionNumbers.CreateConnectionLink, "Adding table for established connections")]
public class Version05 : Migration
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        CreateTableConnection();
    }

    private void CreateTableConnection()
    {
        var table = BaseVersion.InsertStandardColumns(Create.Table("Connections"));

        table
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Connection_User_Id", "Users", "Id")
            .WithColumn("ConnectedWithUserId").AsInt64().NotNullable().ForeignKey("FK_Connection_ConnectedWithUser_Id", "Users", "Id");
    }
}
