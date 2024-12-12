using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI itemAmount;

    Item item;

    // public delegate void OnItemChanged();
    // public OnItemChanged onItemChangedCallback;

    void Start() {
        itemAmount.enabled = false;
    }

    public void AddItem(Item newItem) {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() {
        Inventory.instance.Remove(item);
    }

    public void UseItem() {
        if (item != null) {
            if (item.itemAmount >= 1) {
                item.Use();
                item.itemAmount--;
                itemAmount.text = item.itemAmount.ToString("n0");
                if (item.itemAmount == 0) {
                    ClearSlot();
                    itemAmount.enabled = false;
                }
            }
        }
    }
}
