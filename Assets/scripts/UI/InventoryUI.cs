using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    private bool isOpened = false;
    private Inventory inventory;
    [SerializeField]
    Text stone;
    [SerializeField]
    Text pork;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isOpened)
            {
                isOpened = false;
                inventory.OnInventoryUpdate -= OnInventoryUpdate;
                inventory = null;
                panel.SetActive(false);
            }
            else
            {
                isOpened = true;
                Thing thing = ThingSystem.Instance.FindThingsWithComp(typeof(InvComp))[0];
                InvComp invComp = (InvComp)thing.GetComp(typeof(InvComp));
                inventory = invComp.Inventory;
                inventory.OnInventoryUpdate += OnInventoryUpdate;
                OnInventoryUpdate(inventory);

                panel.SetActive(true);
            }
        }

        if (Player.Instance.stone <= 0) {
            stone.text = "NO stone";
        }
        else stone.text = "stone x" + Player.Instance.stone.ToString();

        if (Player.Instance.pork <= 0) {
            pork.text = "NO pork";
        }
        else pork.text = "pork x" + Player.Instance.pork.ToString();
    }

    private void OnInventoryUpdate(Inventory inventory)
    {
        // text.text = "인벤\n";
        // foreach (Item item in inventory.AllItems())
        //     text.text += item.ItemData.Name + " " + item.amount + "개\n";
    }
}