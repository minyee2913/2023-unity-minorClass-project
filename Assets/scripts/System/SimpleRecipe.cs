using UnityEngine;

public class SimpleRecipe {
    public static void RecipePotion() {
        if (Player.Instance.stone >= 8 && Player.Instance.pork >= 4) {
            Player.Instance.stone -= 8;
            Player.Instance.pork -= 4;

            Player.Instance.potion++;
        }
    }

    public static void RecipePork() {
        if (Player.Instance.stone >= 50) {
            Player.Instance.stone -= 50;
            Player.Instance.pork -= 4;

            Player.Instance.potion++;
        }
    }
}