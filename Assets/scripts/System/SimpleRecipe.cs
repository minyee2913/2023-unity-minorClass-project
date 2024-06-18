using UnityEngine;

public class SimpleRecipe : MonoBehaviour {
    public void RecipePotion() {
        if (Player.Instance.stone >= 8 && Player.Instance.pork >= 4) {
            Player.Instance.stone -= 8;
            Player.Instance.pork -= 4;

            Player.Instance.potion++;
        } else {
            InventoryUI.Instance.Error("재료가 부족합니다!");
        }
    }

    public void RecipePork() {
        if (Player.Instance.stone >= 50) {
            Player.Instance.stone -= 50;

            Player.Instance.pork++;
        } else {
            InventoryUI.Instance.Error("재료가 부족합니다!");
        }
    }

    public void RecipeHamer() {
        if (Player.Instance.stone >= 10 && Player.Instance.pork >= 1) {
            Player.Instance.stone -= 10;
            Player.Instance.pork -= 1;

            Player.Instance.hamer++;
        } else {
            InventoryUI.Instance.Error("재료가 부족합니다!");
        }
    }
}