using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionNumbers.CreateRecipeTable, "Create recipe table")]
public class Version02 : Migration
{
    public override void Down()
    {
    }

    public override void Up()
    {
        CreateTableRecipes();
        CreateTableIngredients();
    }

    private void CreateTableRecipes()
    {
        var table = BaseVersion.InsertStandardColumns(Create.Table("Recipes"));

        table
            .WithColumn("Title").AsString(100).NotNullable()
            .WithColumn("Category").AsInt16().NotNullable()
            .WithColumn("Instructions").AsString(5000).NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id");
    }

    private void CreateTableIngredients()
    {
        var table = BaseVersion.InsertStandardColumns(Create.Table("Ingredients"));

        table
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Measurement").AsString(100).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredient_Recipe_Id", "Recipes", "Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
}
