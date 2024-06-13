using UnityEngine;

public class Recipe {
    public string Item;
    public int Amount;

    public Recipe(string item, int amount) {
        Item = item;
        Amount = amount;
    }
}

public class RecipeData : Data {
    public Recipe[] recipes {get; private set;}
    public RecipeData(Recipe[] wrapper) {
        recipes = wrapper;
    }
}