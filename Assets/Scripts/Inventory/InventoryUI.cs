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
    private static AudioClip equipClip;
    private static AudioClip dequipClip;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        equippedInteractable = null;
    }

    void Update()
    {
        if (Game.ClickDetected() && !InventoryIsOpen())
        {
            if (Player.equipped == Equippable.None)
                EquipInteractable();

            else if (Player.equipped == Equippable.Interactable)
                DequipInteractable();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            AddToInventory();
        }

        // OPEN OR CLOSE the inventory when the Inventory button is clicked
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        // if inventory is ALREADY OPEN and click outside of it, close it
        bool clickedOutsideInventory = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
        if (clickedOutsideInventory && InventoryIsOpen()) // Only close inventory if it's open
        {
            ToggleInventory();
        }
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

        StartCoroutine(Game.PlayAudio("Inventory", 0.4f));
    }

    private static void EquipInteractable()
    {
        var other = InteractableDetected();

        if (other == null) return;

        other.transform.parent = GameObject.Find("InteractableContainer").transform;
        other.transform.localPosition = Vector3.zero;
        Player.equipped = Equippable.Interactable;
        equippedInteractable = other;

        equipClip ??= Array.Find(Game.audioClips, (e) => e.name == "Equip");
        Game.audioSource.PlayOneShot(equipClip, 0.3f);
    }

    private static void DequipInteractable()
    {
        equippedInteractable.transform.parent = null;
        Player.equipped = Equippable.None;
        equippedInteractable = null;

        dequipClip ??= Array.Find(Game.audioClips, (e) => e.name == "Dequip");
        Game.audioSource.PlayOneShot(dequipClip, 0.3f);
    }

    private static GameObject InteractableDetected()
    {
        Collider[] colliders = Physics.OverlapSphere(
            Player.activeArea.transform.position, 0.75f
        );

        return Array.Find(colliders, (c) => c.CompareTag("Interactable"))?.gameObject;
    }

    // check if the inventory is already open
    bool InventoryIsOpen()
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
        bool setActive = !InventoryIsOpen();

        if (setActive) Clock.Pause();
        else Clock.Resume();

        inventoryUI.SetActive(setActive);
        Player.pauseMovement = setActive;

        UpdateUI();
    }
}
