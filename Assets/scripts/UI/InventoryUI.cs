using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance {get; private set;}
    [SerializeField]
    private GameObject panel;
    public bool isOpened = false;
    private Inventory inventory;
    [SerializeField]
    Text stone;
    [SerializeField]
    Text pork;
    [SerializeField]
    Text potion;
    [SerializeField]
    Text hamer;
    [SerializeField]
    Text stone2;
    [SerializeField]
    Text pork2;
    [SerializeField]
    Text potion2;
    [SerializeField]
    Text hamer2;

    [SerializeField]
    Text errorMsg;

    void Awake() {
        Instance = this;
    }

    public void Error(string txt) {
        errorMsg.text = txt;

        Invoke("ErrorEnd", 1.3f);
    }

    void ErrorEnd() {
        errorMsg.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isOpened)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        if (Player.Instance.stone <= 0) {
            stone.text = "NO stone";
        }
        else stone.text = "stone x" + Player.Instance.stone.ToString();

        stone2.text = stone.text;

        if (Player.Instance.pork <= 0) {
            pork.text = "NO pork";
        }
        else pork.text = "pork x" + Player.Instance.pork.ToString();

        pork2.text = pork.text;

        if (Player.Instance.potion <= 0) {
            potion.text = "NO potion";
        }
        else potion.text = "potion x" + Player.Instance.potion.ToString();

        potion2.text = potion.text;

        if (Player.Instance.hamer <= 0) {
            hamer.text = "NO hamer";
        }
        else hamer.text = "hamer x" + Player.Instance.hamer.ToString();

        hamer2.text = hamer.text;
    }

    public void Close() {
        isOpened = false;
        inventory.OnInventoryUpdate -= OnInventoryUpdate;
        inventory = null;
        panel.SetActive(false);
    }

    public void Open() {
        isOpened = true;
        Thing thing = ThingSystem.Instance.FindThingsWithComp(typeof(InvComp))[0];
        InvComp invComp = (InvComp)thing.GetComp(typeof(InvComp));
        inventory = invComp.Inventory;
        inventory.OnInventoryUpdate += OnInventoryUpdate;
        OnInventoryUpdate(inventory);

        panel.SetActive(true);
    }

    private void OnInventoryUpdate(Inventory inventory)
    {
        // text.text = "인벤\n";
        // foreach (Item item in inventory.AllItems())
        //     text.text += item.ItemData.Name + " " + item.amount + "개\n";
    }
}