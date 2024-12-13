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
        bool clickedOutsideInventory = Input.GetMouseButtonDown(0)
            && !EventSystem.current.IsPointerOverGameObject();

        if (
            Input.GetButtonDown("Inventory") ||
            (inventoryUI.activeSelf && clickedOutsideInventory)
        )
            ToggleInventory();
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
                Debug.Log("made it into the for loop...");
                slots[i].itemAmount.enabled = true;
                //Debug.Log("the amount of items is: " + inventory.items[i].itemAmount);
                slots[i].itemAmount.text = inventory.items[i].itemAmount.ToString("n0");
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
        bool setActive = !inventoryUI.activeSelf;

        inventoryUI.SetActive(setActive);
        Player.pauseMovement = setActive;

        UpdateUI();
    }
}
