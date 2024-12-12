using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory")) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI() {
        
        for (int i=0; i<slots.Length; i++) {
            if (i < inventory.items.Count) {
                slots[i].AddItem(inventory.items[i]);
                slots[i].itemAmount.enabled = true;
                Debug.Log("the amount of items is: " + inventory.items[i].itemAmount);
                slots[i].itemAmount.text = inventory.items[i].itemAmount.ToString("n0");
            }
            else {
                slots[i].ClearSlot();
                slots[i].itemAmount.enabled = false;
            }
        }
    }
}
