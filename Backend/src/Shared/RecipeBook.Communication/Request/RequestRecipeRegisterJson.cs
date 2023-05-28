﻿using RecipeBook.Communication.Enum;
namespace RecipeBook.Communication.Request;
public class RequestRecipeRegisterJson
{
    public RequestRecipeRegisterJson()
    {
        Ingredients = new();
    }

    public string Title { get; set; }
    public Category Category { get; set; }
    public string Instructions { get; set; }
    public List<RequestIngredientRegisterJson> Ingredients { get; set; }
}