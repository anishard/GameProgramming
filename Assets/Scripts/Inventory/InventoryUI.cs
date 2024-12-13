using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{

    public GameObject inventoryUI;
    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;
    public static GameObject equippedInteractable;
    public Interactable focus;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        equippedInteractable = null;
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

        if (Game.ClickDetected())
        {
            if (Player.equipped == Equippable.None)
                EquipInteractable();

            else if (Player.equipped == Equippable.Interactable)
                DequipInteractable();
        }

        if (Input.GetKeyDown(KeyCode.U))
            AddToInventory();
    }

    public void AddToInventory()
    {
        Interactable newFocus = equippedInteractable?.GetComponent<Interactable>();

        if (newFocus == null) return;

        if (newFocus != focus)
        {
            if (focus != null) focus.OnDefocused();
            focus = newFocus;
        }

        newFocus.OnFocused();
        Player.equipped = Equippable.None;
        equippedInteractable = null;
    }

    private static void EquipInteractable()
    {
        var other = InteractableDetected();

        if (other == null) return;

        other.transform.parent = GameObject.Find("InteractableContainer").transform;
        other.transform.localPosition = Vector3.zero;
        Player.equipped = Equippable.Interactable;
        equippedInteractable = other;
    }

    private static void DequipInteractable()
    {
        equippedInteractable.transform.parent = null;
        Player.equipped = Equippable.None;
        equippedInteractable = null;
    }

    private static GameObject InteractableDetected()
    {
        Collider[] colliders = Physics.OverlapSphere(
            Player.activeArea.transform.position, 0.75f
        );

        return Array.Find(colliders, (c) => c.CompareTag("Interactable"))?.gameObject;
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
                if (inventory.items[i].itemAmount > 0)
                {
                    slots[i].AddItem(inventory.items[i]);
                    slots[i].itemAmount.enabled = true;
                    slots[i].itemAmount.text = inventory.items[i].itemAmount.ToString("n0");
                }
                else
                {
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
