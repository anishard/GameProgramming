using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    
    public GameObject inventoryUI;
    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        // OPEN OR CLOSE the inventory when the Inventory button is clicked
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory(); 
        }

        // if inventory is ALREADY OPEN and click outside of it, close it
        bool clickedOutsideInventory = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
        if (clickedOutsideInventory && IsInventoryOpen()) // Only close inventory if it's open
        {
            ToggleInventory(); 
        }
    }

    // check if the inventory is already open
    bool IsInventoryOpen()
    {
        return inventoryUI.activeSelf; 
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                if (inventory.items[i].itemAmount > 0) {
                    slots[i].AddItem(inventory.items[i]);
                    slots[i].itemAmount.enabled = true;
                    slots[i].itemAmount.text = inventory.items[i].itemAmount.ToString("n0");
                }
                else {
                    slots[i].ClearSlot();
                    slots[i].itemAmount.enabled = false;
                }
            }
            else
            {
                slots[i].ClearSlot();
                slots[i].itemAmount.enabled = false;
            }
        }
    }

    public void ToggleInventory()
    {
        bool setActive = !IsInventoryOpen();

        if (setActive) Clock.Pause();
        else Clock.Resume();

        inventoryUI.SetActive(setActive);
        Player.pauseMovement = setActive;

        UpdateUI();
    }
}
