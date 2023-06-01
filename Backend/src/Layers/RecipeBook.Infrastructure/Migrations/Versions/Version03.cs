using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionNumbers.UpdateRecipeTable, "Add Preparation Time column")]
public class Version03 : Migration
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        Alter.Table("recipes").AddColumn("PreparationTime")
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(0);
    }
}
