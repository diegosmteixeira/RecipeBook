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
            .WithColumn("Categories").AsInt16().NotNullable()
            .WithColumn("Instructions").AsString(5000).NotNullable();
    }

    private void CreateTableIngredients()
    {
        var table = BaseVersion.InsertStandardColumns(Create.Table("Ingredients"));

        table
            .WithColumn("Ingredient").AsString(100).NotNullable()
            .WithColumn("Measurement").AsInt16().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredient_RecipeId", "Recipes", "Id");
    }
}
