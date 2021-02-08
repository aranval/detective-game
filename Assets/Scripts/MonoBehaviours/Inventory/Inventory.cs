using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public const int itemSlotsNumber = 6;
    public Image[] itemImages = new Image[itemSlotsNumber];
    public Item[] items = new Item[itemSlotsNumber];
    private Image item;

    public void AddItem(Item addItem) {
        
        Debug.Log(message: "Nazwa itemu: " + addItem.name + "; Sprite zalaczony: " + (addItem.sprite != null));
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == null) {
                items[i] = addItem;
                itemImages[i].sprite = addItem.sprite;
                itemImages[i].enabled = true;
                Debug.Log(items[i].name + " pomyślnie dodano.");
                return;
            }
        }
    }

    public void RemoveItem(Item delItem) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == delItem) {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                return;
            }
        }
    }
}