using FluentMigrator.Builders.Create.Table;

namespace RecipeBook.Infrastructure.Migrations;

public static class BaseVersion
{
    public static ICreateTableColumnOptionOrWithColumnSyntax InsertStandardColumns(ICreateTableWithColumnOrSchemaOrDescriptionSyntax table)
    {
        return table
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreatedDate").AsDateTime().NotNullable();
    }
}
