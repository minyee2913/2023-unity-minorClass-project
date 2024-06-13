[System.Serializable]
public class RecipeWrapper {
    public RecipeJson[] recipies;
}

[System.Serializable]
public class RecipeJson {
    public string item;
    public int amount;
}